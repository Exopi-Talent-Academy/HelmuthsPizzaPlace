using PizzaPlace.Models;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

/// <summary>
/// Produces pizza on one giant revolving surface. Downside is, that all pizzas must have the same cooking time, when prepared at the same time.
/// </summary>
public class GiantRevolvingPizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
{
    private const int GiantRevolvingPizzaOvenCapacity = 120;

    protected override int Capacity => GiantRevolvingPizzaOvenCapacity;

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    {
        var queuePizza = recipeOrders.ToList();
        Task previousBatch = Task.CompletedTask;

        while (queuePizza.Count() > 0)
        {
            var cookingTime = queuePizza[0].Recipe.CookingTimeMinutes;

            var batch = queuePizza.Where(item => item.Recipe.CookingTimeMinutes == cookingTime).Take(Capacity).ToList();
            var batchFinished = new TaskCompletionSource<bool>();
            var pizzasRemaining = batch.Count;

            void MarkPizzaAsFinished()
            {
                if (Interlocked.Decrement(ref pizzasRemaining) == 0)
                {
                    batchFinished.TrySetResult(true);
                }
            }

            foreach (var (recipe, orderGuid) in batch)
            {
                _pizzaQueue.Enqueue((MakePizza(recipe, cookingTime, previousBatch, MarkPizzaAsFinished), orderGuid));

                
            }

            // Only the next batch should wait for this batch.
            previousBatch = batchFinished.Task;
            queuePizza.RemoveAll(item => batch.Contains(item));

            /* PREVIOUS ATTEMPT:

            var currentlyCooking = 0;
            var batchOrder = new List<(PizzaRecipeDto Recipe, Guid Guid)>();

            foreach (var (recipe, orderGuid) in queuePizza)
            {
                if (currentlyCooking < Capacity && recipe.CookingTimeMinutes == cookingTime)
                {
                    currentlyCooking += 1;
                    batchOrder.Add((recipe, orderGuid));
                    _pizzaQueue.Enqueue((MakePizza(recipe), orderGuid));
                }
                else
                {
                    continue;
                }
            }
            queuePizza.RemoveAll(item => batchOrder.Contains(item));
            batchOrder.Clear();*/
        }
    }
    private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe, int cookingTime, Task previousBatch, Action markPizzaAsFinished) => async () =>
    {
        await previousBatch;

        try
        {
            await CookPizza(cookingTime);
            return GetPizza(recipe.RecipeType);
        }
        finally
        {
            markPizzaAsFinished();
        }

    };
}

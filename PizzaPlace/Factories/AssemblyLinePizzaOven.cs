using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

/// <summary>
/// Producing one line of pizza. 
/// Taking 7 minutes to setup - and then 5 minutes less for every subsequent pizza of the same recipe type to a minimum of 4 minutes.
/// </summary>
public class AssemblyLinePizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
{
    private const int AssemblyLineCapacity = 1;
    public const int SetupTimeMinutes = 7;
    public const int SubsequentPizzaTimeSavingsInMinutes = 5;
    public const int MinimumCookingTimeMinutes = 4;

    protected override int Capacity => AssemblyLineCapacity;

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    {
        PizzaRecipeType? previousRecipeType = null;
        var assemblyCounter = 0;


        foreach (var (recipe, orderGuid) in recipeOrders)
        {
            if (previousRecipeType != recipe.RecipeType)
            {
                assemblyCounter = 0;
                previousRecipeType = recipe.RecipeType;
            }
            var cookingTime = recipe.CookingTimeMinutes + SetupTimeMinutes;

            if (cookingTime - SubsequentPizzaTimeSavingsInMinutes*assemblyCounter < MinimumCookingTimeMinutes && assemblyCounter > 0)
            {
                cookingTime = MinimumCookingTimeMinutes;
            }
            else
            {
                cookingTime = cookingTime - SubsequentPizzaTimeSavingsInMinutes*assemblyCounter;
            }

            assemblyCounter += 1;
            _pizzaQueue.Enqueue((MakePizza(recipe, cookingTime), orderGuid));
        }
    }
    private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe, int cookingTime) => async () =>
    {
        await CookPizza(cookingTime);

        return GetPizza(recipe.RecipeType);
    };
}

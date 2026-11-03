using PizzaPlace.Extensions;
using PizzaPlace.Models;
using PizzaPlace.Pizzas;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

/// <summary>
/// Checks on current stocks and orders, and takes elements from stock to use for pizzas.
/// </summary>
/// <param name="stockRepository"></param>
public class StockService(IStockRepository stockRepository) : IStockService
{
    /// <summary>
    /// Checks for stock insufficiency with all current orders
    /// </summary>
    /// <param name="order"></param>
    /// <param name="recipeDtos"></param>
    /// <returns></returns>
    /// <exception cref="PizzaException">If a recipe is missing for an ordered pizza.</exception>
    public async Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        var prepareOrders = order.RequestedOrder.Select(request =>  // Problem: GetRequiredStock expects a list of PizzaPrepareOrder, so we need to convert the PizzaOrder we have in order
            {
                var recipe = recipeDtos.FirstOrDefault(
                    recipe => recipe.RecipeType == request.PizzaType);

                if (recipe is null)
                {
                    throw new PizzaException($"Missing recipe for {request.PizzaType}.");
                }
                if (recipe.Ingredients.Any(ingredient => ingredient.Amount <= 0))
                {
                    throw new PizzaException($"{request.PizzaType} has an invalid number of ingredients.");
                }
                return new PizzaPrepareOrder(recipe, request.Amount);
            }).ToList();

        var requiredStock = prepareOrders.GetRequiredStock(); // Now we can get a list of orders.

        // Now we need to compare required stock with the repository stock.
        foreach (var required in requiredStock)
        {
            var available = await stockRepository.GetStock(required.StockType);

            if (available.Amount < required.Amount)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Takes items from stock that are used for orders.
    /// </summary>
    /// <returns></returns>
    public async Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        var prepareOrders = order.RequestedOrder.Select(request =>
            {
                var recipe = recipeDtos.FirstOrDefault(
                    recipe => recipe.RecipeType == request.PizzaType);

                if (recipe is null)
                {
                    throw new PizzaException(
                        $"Missing recipe for {request.PizzaType}.");
                }

                return new PizzaPrepareOrder(recipe, request.Amount);
            }).ToList();

        // Uses the existing methods we find in PizzaHelperExtensions.
        var requiredStock = prepareOrders.GetRequiredStock();

        ComparableList<StockDto> takenStock = [];

        foreach (var required in requiredStock)
        {
            var stock = await stockRepository.TakeStock(
                required.StockType,
                required.Amount);

            takenStock.Add(stock);
        }

        return takenStock;

    }
}

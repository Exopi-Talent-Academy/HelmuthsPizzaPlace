using PizzaPlace.Models;

namespace PizzaPlace.Extensions;

/// <summary>
/// Helper functions to check required stock and whether ingredients are covered by current stock.
/// </summary>
public static class PizzaHelperExtensions
{
        /// <summary>
        /// Calculates the required stock needed to prepare all orders currently in the system.
        /// </summary>
        /// <param name="prepareOrders">List of pizza orders and quantities to prepare.</param>
        /// <returns>
        /// List of required stock by stock type, run through GathersSameTypeOfStock()
        /// </returns>
    public static IEnumerable<StockDto> GetRequiredStock(
        this IEnumerable<PizzaPrepareOrder> prepareOrders) =>  

        prepareOrders.SelectMany(                           // SelectMany flattens the orders that are found below
            order => Enumerable.Range(0, order.OrderAmount)
            .Select(_ => order.RecipeDto)) 
        .SelectMany(recipe => recipe.Ingredients)           // Flattens the order ingredients (so instead of separate list per pizza, one collapsed list)
        .GatherSameTypeOfStock();


        /// <summary>
        /// Combines the stock entries of the same type into groups and sums up their amounts.
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
    public static IEnumerable<StockDto> GatherSameTypeOfStock(this IEnumerable<StockDto> stock) =>
        stock.GroupBy(stock => stock.StockType)
        .Select(stockGroup => new StockDto(stockGroup.Key, stockGroup.Sum(x => x.Amount)));

        /// <summary>
        /// Function for checking if current stock covers needed stock for all orders.
        /// </summary>
        /// <returns></returns>
    public static bool HasEnough(this IEnumerable<StockDto> availableStock, IEnumerable<StockDto> requiredStock) =>
        requiredStock.GatherSameTypeOfStock()
        .All(required => availableStock.Any(x => x.StockType == required.StockType && x.Amount >= required.Amount));
}

using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Controllers;

[Route("api/restocking")]
public class RestockingController(IStockRepository stockRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Restock([FromBody] ComparableList<StockDto> stock)
    {
        ComparableList<StockDto> restocked = [];

        foreach (var stockItem in stock)
        {
            var updatedStock = await stockRepository.AddToStock(stockItem);
            restocked.Add(updatedStock);
        }

        return Ok(restocked);
    }
}

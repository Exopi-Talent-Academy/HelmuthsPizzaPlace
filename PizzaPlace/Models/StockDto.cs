using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

/// <summary>
/// Simple DTO record that holds a quantity for a specific stock type.
/// </summary>
public record StockDto(StockType StockType, int Amount, long Id = 0) : Dto(Id);

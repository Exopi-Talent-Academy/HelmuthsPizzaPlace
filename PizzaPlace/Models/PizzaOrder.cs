using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

/// <summary>
/// Represents a pizza order, with each pizza defined by type and quantity ordered.
/// </summary>
/// <param name="RequestedOrder"></param>
public record PizzaOrder(ComparableList<PizzaAmount> RequestedOrder);

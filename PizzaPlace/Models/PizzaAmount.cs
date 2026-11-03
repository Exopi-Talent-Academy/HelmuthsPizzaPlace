using PizzaPlace.Models.Types;

namespace PizzaPlace;

/// <summary>
/// Describes the type of pizza and amount ordered of said type.
/// </summary>
public record PizzaAmount(PizzaRecipeType PizzaType, ushort Amount);

using PizzaPlace.Models.Types;

namespace PizzaPlace;

public record MenuItem(string Name, string Description, PizzaRecipeType PizzaType, double Price, bool SoldOut = false);

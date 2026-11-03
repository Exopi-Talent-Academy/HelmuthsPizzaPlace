using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

/// <summary>
/// DTO for the pizza recipes, detailing ingredients and cooking time.
/// </summary>
public record PizzaRecipeDto(PizzaRecipeType RecipeType, ComparableList<StockDto> Ingredients, int CookingTimeMinutes, long Id = 0) : Dto(Id);

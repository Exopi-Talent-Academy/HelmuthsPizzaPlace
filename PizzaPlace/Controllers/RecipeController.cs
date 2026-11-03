using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Controllers;

[Route("api/recipes")]
public class RecipeController(IRecipeRepository recipeRepository) : ControllerBase
{
    [HttpGet("{recipeType}")]
    public async Task<IActionResult> GetRecipe(PizzaRecipeType recipeType)
    {
        var recipe = await recipeRepository.GetRecipe(recipeType);

        return Ok(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> AddRecipe(
        [FromBody] PizzaRecipeDto recipe)
    {
        var id = await recipeRepository.AddRecipe(recipe);
        var createdRecipe = recipe with { Id = id };

        return CreatedAtAction(
            nameof(GetRecipe),
            new { recipeType = createdRecipe.RecipeType },
            createdRecipe);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRecipe(
        [FromBody] PizzaRecipeDto recipe)
    {
        var updatedRecipe = await recipeRepository.UpdateRecipe(recipe);

        return Ok(updatedRecipe);
    }
}

/* The endpoints are
GET  /api/recipes/StandardPizza
POST /api/recipes
PUT  /api/recipes


An update request must include the recipe ID:
{
  "id": 1,
  "recipeType": "StandardPizza",
  "ingredients": [
    {
      "stockType": "Dough",
      "amount": 1
    },
    {
      "stockType": "Tomatoes",
      "amount": 2
    }
  ],
  "cookingTimeMinutes": 10
}

*/
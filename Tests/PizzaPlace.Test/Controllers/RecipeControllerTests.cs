using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class RecipeControllerTests
{
    [TestMethod]
    public async Task AddRecipe_ReturnsCreatedRecipe()
    {
        // Arrange
        var recipe = new PizzaRecipeDto(
            PizzaRecipeType.StandardPizza,
            [
                new StockDto(StockType.Dough, 1),
                new StockDto(StockType.Tomatoes, 2)
            ],
            10);

        var recipeRepository = new Mock<IRecipeRepository>(
            MockBehavior.Strict);

        recipeRepository
            .Setup(repository => repository.AddRecipe(recipe))
            .ReturnsAsync(42);

        var controller = new RecipeController(
            recipeRepository.Object);

        // Act
        var result = await controller.AddRecipe(recipe);

        // Assert
        var createdResult = result as CreatedAtActionResult;

        Assert.IsNotNull(createdResult);
        Assert.AreEqual(nameof(RecipeController.GetRecipe),
            createdResult.ActionName);

        var returnedRecipe =
            createdResult.Value as PizzaRecipeDto;

        Assert.IsNotNull(returnedRecipe);
        Assert.AreEqual(42, returnedRecipe.Id);
        Assert.AreEqual(recipe.RecipeType, returnedRecipe.RecipeType);

        recipeRepository.VerifyAll();
    }

    [TestMethod]
    public async Task UpdateRecipe_ReturnsUpdatedRecipe()
    {
        // Arrange
        var recipe = new PizzaRecipeDto(
            PizzaRecipeType.StandardPizza,
            [
                new StockDto(StockType.Dough, 2),
                new StockDto(StockType.Tomatoes, 3)
            ],
            12,
            42);

        var recipeRepository = new Mock<IRecipeRepository>(
            MockBehavior.Strict);

        recipeRepository
            .Setup(repository => repository.UpdateRecipe(recipe))
            .ReturnsAsync(recipe);

        var controller = new RecipeController(
            recipeRepository.Object);

        // Act
        var result = await controller.UpdateRecipe(recipe);

        // Assert
        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);

        var returnedRecipe =
            okResult.Value as PizzaRecipeDto;

        Assert.IsNotNull(returnedRecipe);
        Assert.AreEqual(recipe, returnedRecipe);

        recipeRepository.VerifyAll();
    }
}
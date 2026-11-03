using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class StockServiceTests
{
    private readonly Mock<IStockRepository> stockRepositoryMock;
    private readonly StockService stockService;
    public StockServiceTests()
    {
        stockRepositoryMock = new Mock<IStockRepository>(MockBehavior.Strict);
        stockService = new StockService(stockRepositoryMock.Object);
    }

    private void SetupStock(StockType stockType, int amount)
    {
        stockRepositoryMock
            .Setup(repository => repository.GetStock(stockType))
            .ReturnsAsync(new StockDto(stockType, amount));
    }

    [TestMethod]
    public async Task HasInsufficientStock_WhenEnoughStockExists_ReturnsFalse()
    {
        // Arrange
        SetupStock(StockType.Dough, 2);
        SetupStock(StockType.Tomatoes, 4);

        var order = new PizzaOrder(
        [
            new PizzaAmount(PizzaRecipeType.StandardPizza, 2)
        ]);

        ComparableList<PizzaRecipeDto> recipes =
        [
            new PizzaRecipeDto(
                PizzaRecipeType.StandardPizza,
                [
                    new StockDto(StockType.Dough, 1),
                    new StockDto(StockType.Tomatoes, 2)
                ],
                10)
        ];

        // Act
        var result = await stockService.HasInsufficientStock(order, recipes);

        // Assert
        Assert.IsFalse(result);
    }


    [TestMethod]
    public async Task GetStock_TakesRequiredIngredientsAndReturnsTakenStock()
    {
        // Arrange
        var order = new PizzaOrder(
        [
            new PizzaAmount(PizzaRecipeType.StandardPizza, 2)
        ]);

        ComparableList<PizzaRecipeDto> recipes =
        [
            new PizzaRecipeDto(
            PizzaRecipeType.StandardPizza,
            [
                new StockDto(StockType.Dough, 1),
                new StockDto(StockType.Tomatoes, 2)
            ],
            10)
        ];

        var stockRepository = new Mock<IStockRepository>(
            MockBehavior.Strict);

        stockRepository
            .Setup(repository => repository.GetStock(StockType.Dough))
            .ReturnsAsync(new StockDto(StockType.Dough, 2));

        stockRepository
            .Setup(repository => repository.GetStock(StockType.Tomatoes))
            .ReturnsAsync(new StockDto(StockType.Tomatoes, 4));

        var service = new StockService(stockRepository.Object);

        // Act
        var result = await service.HasInsufficientStock(order, recipes);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task HasInsufficientStock_EmptyRecipe_ThrowsPizzaException()
    {
        // Arrange
        var order = new PizzaOrder(
        [
            new PizzaAmount(PizzaRecipeType.StandardPizza, 2)
        ]);

        ComparableList<PizzaRecipeDto> recipes =
        [
            new PizzaRecipeDto(
                PizzaRecipeType.StandardPizza,
                [
                    
                ],
                10)
        ];
        // Act
        var result = await stockService.HasInsufficientStock(order, recipes);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task HasInsufficientStock_RecipeRequiresZeroOfIngredient_ThrowsPizzaException()
    {
        // Arrange
        var order = new PizzaOrder(
        [
            new PizzaAmount(PizzaRecipeType.StandardPizza, 2)
        ]);

        ComparableList<PizzaRecipeDto> recipes =
        [
            new PizzaRecipeDto(
                PizzaRecipeType.StandardPizza,
                [
                    new StockDto(StockType.Dough, 0),
                    new StockDto(StockType.Tomatoes, 0)
                ],
                10)
        ];
        // Act
        var exception = await Assert.ThrowsExceptionAsync<PizzaException>(
            () => stockService.HasInsufficientStock(order, recipes));

        // Assert
        Assert.AreEqual("StandardPizza has an invalid number of ingredients.", exception.Message);
    }

}
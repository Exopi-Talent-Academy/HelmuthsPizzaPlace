using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class RestockingControllerTests
{
    [TestMethod]
    public async Task Restock_AddsAllStockItemsAndReturnsUpdatedStock()
    {
        // Arrange
        ComparableList<StockDto> stock =
        [
            new StockDto(StockType.Dough, 20),
            new StockDto(StockType.Tomatoes, 15)
        ];

        ComparableList<StockDto> expected =
        [
            new StockDto(StockType.Dough, 20),
            new StockDto(StockType.Tomatoes, 15)
        ];

        var stockRepository = new Mock<IStockRepository>(
            MockBehavior.Strict);

        stockRepository
            .Setup(repository => repository.AddToStock(
                new StockDto(StockType.Dough, 20, 0)))
            .ReturnsAsync(new StockDto(StockType.Dough, 20));

        stockRepository
            .Setup(repository => repository.AddToStock(
                new StockDto(StockType.Tomatoes, 15, 0)))
            .ReturnsAsync(new StockDto(StockType.Tomatoes, 15));

        var controller = new RestockingController(
            stockRepository.Object);

        // Act
        var actionResult = await controller.Restock(stock);

        // Assert
        var okResult = actionResult as OkObjectResult;

        Assert.IsNotNull(okResult);

        var actual = okResult.Value as ComparableList<StockDto>;

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);

        stockRepository.Verify(repository =>
            repository.AddToStock(
                new StockDto(StockType.Dough, 20, 0)),
            Times.Once);

        stockRepository.Verify(repository =>
            repository.AddToStock(
                new StockDto(StockType.Tomatoes, 15, 0)),
            Times.Once);
    }
}
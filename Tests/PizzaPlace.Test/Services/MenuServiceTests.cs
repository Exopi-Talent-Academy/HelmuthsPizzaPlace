using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;


[TestClass]
public class MenuServiceTests
{
    [TestMethod]
    public void GetMenu_DuringLunchHours_ReturnsLunchMenu()
    {
        // Arrange
        var menuService = new MenuService();
        var menuDate = new DateTimeOffset(2026, 10, 26, 12, 0, 0, TimeSpan.Zero); // 12:00 UTC

        // Act
        var menu = menuService.GetMenu(menuDate);

        // Assert
        Assert.AreEqual("Lunch Menu (save money!)", menu.Title);
    }

    [TestMethod]
    public void GetMenu_AfterLunchHours_ReturnsStandardMenu()
    {
        // Arrange
        var menuService = new MenuService();
        var menuDate = new DateTimeOffset(2026, 10, 26, 15, 0, 0, TimeSpan.Zero); // 15:00 UTC

        // Act
        var menu = menuService.GetMenu(menuDate);

        //Assert
        Assert.AreEqual("Standard Menu", menu.Title);
    }

}

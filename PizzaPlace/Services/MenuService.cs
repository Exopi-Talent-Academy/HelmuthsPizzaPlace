using PizzaPlace.Pizzas;
using PizzaPlace.Models.Types; // ADDED so we can designate the PizzaRecipeType for the menu items

namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    public Menu GetMenu(DateTimeOffset menuDate)
    {
        var hour = menuDate.UtcDateTime.Hour;

        if (hour < 14 && hour >= 11) // MHM: Got to update this once we understand how the time is implemented.
        {
            return CreateLunchMenu();
        }

        return CreateStandardMenu(); // If the time above is not raised, we get the normal menu

    }

    private static Menu CreateLunchMenu()
    {
        return new Menu(
            "Lunch Menu (save money!)",
            new ComparableList<MenuItem>
            {
                new("Margherita", "Cheese, Tomato, Basil", PizzaRecipeType.StandardPizza, 6.95),
                new("Diavola", "Cheese, Tomato, Pepperoni", PizzaRecipeType.StandardPizza, 7.95),
                new("The Helmuth Special", "Cheese, Tomato, A Sprinkle of Moondust, Sliced eggs, Spinach, Spoiled onions", PizzaRecipeType.ExtremelyTastyPizza, 10.95),
                new("Pain Desire", "Cheese, Boiled Screws, Iceberg Salad", PizzaRecipeType.OddPizza, 8.95),
                new("Italian Tears", "Pineapple, Cheese, Roasted Pineapple, Tomato, Shaken Pineapple, Oregano, Pineapple Paste", PizzaRecipeType.RarePizza, 9.50),
                new("Integration Disaster", "Haggis, Cheese, Calamari, Sauerkraut, Surströmming, Fermented Shark Fin, Olives, Roasted Pierogi", PizzaRecipeType.RarePizza, 11.50),
                new("Margherita Supreme", "Cheese, Tomato, Cheese, Cheese, Cheese, Cheese, Cheese, Cheese, Cheese", PizzaRecipeType.ExtremelyTastyPizza, 8.95),
                new("I've Got Somewhere To Be", "Tomato", PizzaRecipeType.EmptyPizza, 4.95)
            });
    }

    private static Menu CreateStandardMenu()
    {
        return new Menu(
            "Standard Menu",
            new ComparableList<MenuItem>
            {
                new("Margherita", "Cheese, Tomato, Basil", PizzaRecipeType.StandardPizza, 9.95),
                new("Diavola", "Cheese, Tomato, Pepperoni", PizzaRecipeType.StandardPizza, 10.95),
                new("The Helmuth Special", "Cheese, Tomato, A Sprinkle of Moondust, Sliced eggs, Spinach, Spoiled onions", PizzaRecipeType.ExtremelyTastyPizza, 14.95),
                new("Pain Desire", "Cheese, Boiled Screws, Iceberg Salad", PizzaRecipeType.OddPizza, 11.95),
                new("Italian Tears", "Pineapple, Cheese, Roasted Pineapple, Tomato, Shaken Pineapple, Oregano, Pineapple Paste", PizzaRecipeType.RarePizza, 12.50),
                new("Integration Disaster", "Haggis, Cheese, Calamari, Sauerkraut, Surströmming, Fermented Shark Fin, Olives, Roasted Pierogi", PizzaRecipeType.RarePizza, 14.50),
                new("Margherita Supreme", "Cheese, Tomato, Cheese, Cheese, Cheese, Cheese, Cheese, Cheese, Cheese", PizzaRecipeType.ExtremelyTastyPizza, 11.95),
                new("I've Got Somewhere To Be", "Tomato", PizzaRecipeType.EmptyPizza, 7.95)
            });
    }
}
 
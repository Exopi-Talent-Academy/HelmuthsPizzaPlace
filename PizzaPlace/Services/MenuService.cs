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
            return CreateMenu(true);
        }

        return CreateMenu(false); // If the time above is not raised, we get the normal menu

    }


    private static Menu CreateMenu(bool isLunch)
    {
        var menuTitle = isLunch ? "Lunch Menu (save money!)" : "Standard Menu";

        var price = (double standardPrice) => standardPrice - (isLunch ? 3.00 : 0.00);

        return new Menu(
            menuTitle,
            new ComparableList<MenuItem>
            {
                new("Margherita", "Cheese, Tomato, Basil",
                    PizzaRecipeType.StandardPizza, price(6.95)),
                new("Diavola", "Cheese, Tomato, Pepperoni",
                    PizzaRecipeType.StandardPizza, price(7.95)),
                new("The Helmuth Special", "Cheese, Tomato, A Sprinkle of Moondust, Sliced eggs, Spinach, Spoiled onions",
                    PizzaRecipeType.ExtremelyTastyPizza, price(10.95)),
                new("French Favourite", "Cheese, Tomato, Onions, ",
                    PizzaRecipeType.OddPizza, price(8.95)),
                new("Italian Tears", "Pineapple, Cheese, Roasted Pineapple, Tomato, Shaken Pineapple, Oregano, Pineapple Paste",
                    PizzaRecipeType.RarePizza, price(9.50)),
                new("European Integration Disaster", "Haggis, Cheese, Calamari, Sauerkraut, Surströmming, Fermented Shark Fin, Olives, Roasted Pierogi",
                    PizzaRecipeType.RarePizza, price(11.50)),
                new("Margherita Supreme", "Cheese, Tomato, Cheese, Cheese, Cheese, Cheese, Cheese, Cheese, Cheese",
                    PizzaRecipeType.ExtremelyTastyPizza, price(8.95)),
                new("I've Got Somewhere To Be", "Tomato",
                    PizzaRecipeType.EmptyPizza, price(4.95)),
                new("The poopiboi special", "Cheese, Tomato, Cocktail Sausages, Uncut Mushrooms, Gravy",
                    PizzaRecipeType.OddPizza, price(10.50)),
                new("Britain's Best", "Cheese, Tomato, Baked Beans, Fish, Chips",
                    PizzaRecipeType.OddPizza, price(7.95)),
                new("Trendy & Tragic", "Cheese, Tomato, Raw Fish, Soy",
                    PizzaRecipeType.RarePizza, price(11.50)),
                new("The Alternative", "Goat Cheese, Sliced Potato, Basil Leaves, Mushrooms",
                    PizzaRecipeType.RarePizza, price(9.50))
            });
    }
}
 
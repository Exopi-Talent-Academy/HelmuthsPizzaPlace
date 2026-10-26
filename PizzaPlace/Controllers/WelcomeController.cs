using Microsoft.AspNetCore.Mvc;

namespace PizzaPlace.Controllers;

[Route("api/welcome")]
public class WelcomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Greet()
    {
        Console.WriteLine("Greeted guest.");

        return Ok("Welcome to Helmuth's Pizza Palace! We serve the best pizza in town, and keep things neatly organized in our in-house pizza app. Nothing beats pizza from Helmuth's!");
    }
}

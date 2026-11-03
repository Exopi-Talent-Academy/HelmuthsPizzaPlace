using PizzaPlace;
using PizzaPlace.Factories;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

// For step 5, we add database support.
using Microsoft.EntityFrameworkCore;
using PizzaPlace.Data;

var builder = WebApplication.CreateBuilder(args);   // Builder is an ASP.NET application builder.

builder.Services.AddCors(o =>                       // CORS is a browser security feature
{
    o.AddPolicy("allowAll",
        policy =>
        {
            policy.WithOrigins(
            // "whatEver.homepage.com"
            );
            policy.AllowAnyHeader();
            policy.AllowCredentials();
            policy.AllowAnyMethod();                // Permits the API calls like GET, POST, PUT, DELETE, PATCH
        });
});

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(d =>
{
    d.Title = "Helmuth's Pizza Place";
    d.Version = "v0.0.2";
});

// Register services:
var services = builder.Services;                // Creates a shorter variable name for the Dependency Injection (DI) container, where we add Transients 
services.AddSingleton(TimeProvider.System);     // Create a single TimeProvider object and reuse it.

services.AddScoped<IStockRepository, PostgresStockRepository>();     // IStockRepository --> FakeStockRepository. So when we construct f.e "public StockService(IStockRepository repository)", "new FakeStockRepository()" is created.
services.AddTransient<IRecipeRepository, FakeRecipeRepository>();   //      This is to allow .NET to construct our objects when controllers ask for them.

services.AddTransient<IPizzaOven, GiantRevolvingPizzaOven>();       // ALL THREE OVENS use IPizzaOven as an interface
services.AddTransient<IPizzaOven, AssemblyLinePizzaOven>();         //   This means we can ask for IEnumerable<IPizzaOven> and get
services.AddTransient<IPizzaOven, NormalPizzaOven>();               //   [ NormalPizzaOven, AssemblyLinePizzaOven, GiantRevolvingPizzaOven ]


services.AddTransient<IStockService, StockService>();               // Same as above.
services.AddTransient<IRecipeService, RecipeService>();             // We are linking the interface with the objects.
services.AddTransient<IOrderingService, OrderingService>();         // Lots of Dependency Injection!
services.AddTransient<IMenuService, MenuService>();

// More database stuff to add:
var connectionString = builder.Configuration.GetConnectionString("PizzaDatabase") ?? throw new InvalidOperationException("Connect string 'PizzaDatabase' was not found.");
services.AddDbContext<PizzaDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();                      // Build call for the app

// Feel like adding some initial stock:
using (var scope = app.Services.CreateScope())
{
    var stockRepository = scope.ServiceProvider.GetRequiredService<IStockRepository>();

    ComparableList<StockDto> initialStock = 
        [
        new StockDto(StockType.Dough, 20),
        new StockDto(StockType.Tomatoes, 20),
        new StockDto(StockType.GratedCheese, 20),
        new StockDto(StockType.GenericSpices, 20),
        new StockDto(StockType.UngratedCheese, 20),
        new StockDto(StockType.BellPeppers, 20),
        new StockDto(StockType.Bacon, 20)
        ];

    foreach (var stock in initialStock)
    {
        await stockRepository.AddToStock(stock);
    }


}

    app.UseOpenApi();                               // OpenAPI produces machine-readable API documentation.
app.UseSwaggerUi();                             // Swagger UI creates a website to experiment with the API.

app.UseCors();                                  // Uses CORS to control who can call our API.

app.MapControllers();                           // ASP.NET should find all controllers and expose their endpoints. 
                                                //  These endpoints are defined by their Routes, e.g [Route("api/restocking")]

app.Run();                                      // Conceptually a while loop. Now we are running!

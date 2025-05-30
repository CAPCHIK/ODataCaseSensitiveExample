using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Product>("Product");
modelBuilder.EntityType<Product>().HasKey(p => p.Id);

builder.Services.AddControllers().AddOData(
      options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
          "odata",
          modelBuilder.GetEdmModel()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseODataRouteDebug();

app.MapControllers();

app.Run();

public class ProductController : ODataController
{
    private static readonly List<Product> products =
    [
        new Product { Id = 1, Name = "Product 1", Price = 10.0M, ID = "some" },
        new Product { Id = 2, Name = "Product 2", Price = 20.0M, ID = null }
    ];

    [EnableQuery]
    public ActionResult<IEnumerable<Product>> Get()
    {
        return Ok(products);
    }

    [EnableQuery]
    public ActionResult<Product> Get(int key)
    {
        var product = products.FirstOrDefault(p => p.Id == key);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
}

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    /// <summary>
    /// Some old but required field
    /// </summary>
    public string? ID { get; set; }
}

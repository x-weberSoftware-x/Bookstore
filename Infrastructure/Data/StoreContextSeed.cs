using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        //make sure there are no products already
        if (!context.Products.Any())
        {
            //if there are no products yet then seed our dummy data (go up and out first so we get the specific path)
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            //deserialize the json
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            //if products are null return out of here
            if (products == null) return;

            //add these productsa to be tracked by EF
            context.Products.AddRange(products);

            //save
            await context.SaveChangesAsync();
        }
    }
}

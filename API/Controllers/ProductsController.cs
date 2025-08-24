using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

//this helps us to not have to tell it where the body is so we get automatic model binding so we can jkust pass in 'Product product' instead of adding FromBody
[ApiController]
//says to use the name of the controller minus the word 'Controller' so in this case 'Products'
[Route("api/[controller]")]
public class ProductsController(StoreContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        //return a list of the products
        return await context.Products.ToListAsync();
    }

    [HttpGet("{id:int}")] //api/products/id
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        //get the product by the id
        var product = await context.Products.FindAsync(id);

        //product is null so return notfound error (404)
        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        //add product
        context.Products.Add(product);
        //tell EF to track this new product
        await context.SaveChangesAsync();

        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        //make sure the id mathches and that the product exists
        if (product.Id != id || !ProductExists(id)) return BadRequest("Cannmot update this product");

        //tell EF that this is a product that has been modified so it knows to track it
        context.Entry(product).State = EntityState.Modified;

        //tell EF to track the product
        await context.SaveChangesAsync();

        //since this is an update we dont return anything so return NoContent
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null) return NotFound();

        context.Products.Remove(product);

        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        //will return true or false depending on if product with this id exists in db
        return context.Products.Any(x => x.Id == id);
    }
}

using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

//this helps us to not have to tell it where the body is so we get automatic model binding so we can jkust pass in 'Product product' instead of adding FromBody
[ApiController]
//says to use the name of the controller minus the word 'Controller' so in this case 'Products'
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? author, string? genre, string? sort)
    {
        //return a list of the products
        return Ok(await repo.GetProductsAsync(author, genre, sort));
    }

    [HttpGet("{id:int}")] //api/products/id
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        //get the product by the id
        var product = await repo.GetProductByIdAsync(id);

        //product is null so return notfound error (404)
        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        //add product
        repo.AddProduct(product);
        
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        //make sure the id mathches and that the product exists
        if (product.Id != id || !ProductExists(id)) return BadRequest("Cannmot update this product");

        repo.UpdateProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        repo.DeleteProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting product");
    }

    [HttpGet("authors")] // api/products/authors
    public async Task<ActionResult<IReadOnlyList<string>>> GetAuthors()
    {
        return Ok(await repo.GetAuthorsAsync());
    }

    [HttpGet("genres")] // api/products/genres
    public async Task<ActionResult<IReadOnlyList<string>>> GetGenres()
    {
        return Ok(await repo.GetGenresAsync());
    }

    private bool ProductExists(int id)
    {
        //will return true or false depending on if product with this id exists in db
        return repo.ProductExists(id);
    }
}

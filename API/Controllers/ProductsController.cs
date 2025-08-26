using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

//this helps us to not have to tell it where the body is so we get automatic model binding so we can jkust pass in 'Product product' instead of adding FromBody
[ApiController]
//says to use the name of the controller minus the word 'Controller' so in this case 'Products'
[Route("api/[controller]")]
//use the generic repository but give it a type of product since this is the products controller
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? author, string? genre, string? sort)
    {
        //use our specification for filtering, sorting, etc
        
        //first create an instance of our product specification and pass in the author and genre so it filters by author and genre if they are passed in the GET
        var spec = new ProductSpecification(author, genre, sort);
        //then use our repo to get a list of products based on the query(spec) created above
        var products = await repo.ListAsync(spec);

        //return the list of the products
        return Ok(products);
    }

    [HttpGet("{id:int}")] //api/products/id
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        //get the product by the id
        var product = await repo.GetByIdAsync(id);

        //product is null so return notfound error (404)
        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        //add product
        repo.Add(product);
        
        if (await repo.SaveAllAsync())
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

        repo.Update(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        repo.Remove(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting product");
    }

    [HttpGet("authors")] // api/products/authors
    public async Task<ActionResult<IReadOnlyList<string>>> GetAuthors()
    {   
        //create a new instance of our Author list spec
        var spec = new AuthorListSpecification();
        //return a list using the spec so it will return all disinct authors
        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("genres")] // api/products/genres
    public async Task<ActionResult<IReadOnlyList<string>>> GetGenres()
    {
        //create a new instance of our Genre list spec
        var spec = new GenreListSpecification();
        //return a list using the spec so it will return alldisinct genres
        return Ok(await repo.ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        //will return true or false depending on if product with this id exists in db
        return repo.Exists(id);
    }
}

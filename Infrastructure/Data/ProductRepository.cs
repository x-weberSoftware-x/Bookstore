using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetAuthorsAsync()
    {
        //get a distinct list of all the authors
        return await context.Products.Select(x => x.Author).Distinct().ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetGenresAsync()
    {
        //get a distinct list of all the genres
        return await context.Products.Select(x => x.Genre).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? author, string? genre, string? sort)
    {
        // FILTER LIST RETURNED //

        //first make a query that can be manipulated that contains all products
        var query = context.Products.AsQueryable();

        //if a author was passed in then filter all books that match that author to the query
        if (!string.IsNullOrWhiteSpace(author)) query = query.Where(x => x.Author == author);

        //if a genre was passed in then filter all books that match that genre to the query
        if (!string.IsNullOrWhiteSpace(genre)) query = query.Where(x => x.Genre == genre);

        //sort our query
        query = sort switch
        {
            //price ascending if passed in
            "priceAsc" => query.OrderBy(x => x.Price),
            //price descending if passed in
            "priceDesc" => query.OrderByDescending(x => x.Price),
            //default sort by name
            _ => query.OrderBy(x => x.Name)
        };
        

        //return the list of products based on the query
        return await query.ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        //return whether or not EF has changes staged so we can return true or false
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }
}

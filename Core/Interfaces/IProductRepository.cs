using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    //add optional paramters to filter by author or genre
    Task<IReadOnlyList<Product>> GetProductsAsync(string? author, string? genre, string? sort);
    Task<Product?> GetProductByIdAsync(int id);
    Task<IReadOnlyList<string>> GetAuthorsAsync();
    Task<IReadOnlyList<string>> GetGenresAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool ProductExists(int id);
    Task<bool> SaveChangesAsync();
}

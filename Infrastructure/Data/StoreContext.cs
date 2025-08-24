using System.Reflection.Emit;
using Core.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class StoreContext(DbContextOptions options) : DbContext(options)
{
    //this tells EF to call the table 'Products'
    public DbSet<Product> Products { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //tell EF to use our ProductConfiguration class for the configurations
        modelBuilder.ApplyConfigurationsFromAssembly( typeof(ProductConfiguration).Assembly);
    }
}

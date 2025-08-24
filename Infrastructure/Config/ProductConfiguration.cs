using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        //configure our price property so EF knows its 2 place accuracy
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
    }
}

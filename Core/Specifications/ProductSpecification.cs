using System;
using Core.Entities;

namespace Core.Specifications;

//this is in charge of doing the filtering, sorting, pagination etc for the product
public class ProductSpecification : BaseSpecification<Product>
{
    //this constructor takes a author and genre, we then pass those through to the basespecification since it takes expressions(queries)
    //so we are telling the base that the expressions are to filter by author if not null and genre if not null
    public ProductSpecification(string? author, string? genre, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(author) || x.Author == author) &&
        (string.IsNullOrWhiteSpace(genre) || x.Genre == genre)
    )
    {
        //sort the products based on what sort is passed in the GET
        switch (sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}

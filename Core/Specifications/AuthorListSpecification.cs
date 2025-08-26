using System;
using Core.Entities;

namespace Core.Specifications;

public class AuthorListSpecification : BaseSpecification<Product, string>
{
    public AuthorListSpecification()
    {
        AddSelect(x => x.Author);
        ApplyDistinct();
    }
}

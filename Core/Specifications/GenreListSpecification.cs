using System;
using Core.Entities;

namespace Core.Specifications;

public class GenreListSpecification : BaseSpecification<Product, string>
{
     public GenreListSpecification()
    {
        AddSelect(x => x.Genre);
        ApplyDistinct();
    }
}

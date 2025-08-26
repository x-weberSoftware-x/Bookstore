using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    //Where query expression : This created expression is a funtion that takes a type T and returns a bool. The funtion is called Criteria and is a get only
    Expression<Func<T, bool>>? Criteria { get; }

    //OrderBy query expression : the function takes an object since we dont know if it will be a decimal or a string etc that needs to be ordered so this covers all
    Expression<Func<T, object>>? OrderBy { get; }

    //OrderByDescending query expression
    Expression<Func<T, object>>? OrderByDescending { get; }
    bool IsDistinct { get; }
}

//this interface returns a TResult
public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select { get; }
}

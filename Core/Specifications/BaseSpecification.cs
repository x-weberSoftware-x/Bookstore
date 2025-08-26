using System;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

//the primary constructor wich takes in an expression (the exporession will essentially be a where query since that is what our criteria function was made for)
public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    //this is a constructor that allows to create a specification without sending through a expression like the primary constructor requires
    protected BaseSpecification() : this(null) { }

    //our where expression sets itself to the criteria passed in on creation of a new instance
    public Expression<Func<T, bool>>? Criteria => criteria;

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public bool IsDistinct { get; private set; }

    // this function sets the OrderBy above
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    // this function sets the OrderByDescending above
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected void ApplyDistinct()
    {
        IsDistinct = true;
    }
}

//this class is for projection so we can select all genres or all authors, we have to make this because we need to take in two paramters instead of the one above
public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria) : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    //this is a constructor that allows to create a specification without sending through a expression like the primary constructor requires
    protected BaseSpecification() : this(null!) { }

    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}

using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        // the spec.Criteria will be a query like x => x.Name == name that works with the Where by the time it gets to this method
        if (spec.Criteria != null) query = query.Where(spec.Criteria);

        //check if the spec contains an orderby if it does then order the query by it
        if (spec.OrderBy != null) query = query.OrderBy(spec.OrderBy);

        //check if the spec contains an orderByDescending if it does then order the query by it
        if (spec.OrderByDescending != null) query = query.OrderByDescending(spec.OrderByDescending);

        //check if the spec is distinct is set if it is then make the query distinct
        if (spec.IsDistinct) query = query.Distinct();

        //now return the query we modified
        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        // the spec.Criteria will be a query like x => x.Name == name that works with the Where by the time it gets to this method
        if (spec.Criteria != null) query = query.Where(spec.Criteria);

        //check if the spec contains an orderby if it does then order the query by it
        if (spec.OrderBy != null) query = query.OrderBy(spec.OrderBy);

        //check if the spec contains an orderByDescending if it does then order the query by it
        if (spec.OrderByDescending != null) query = query.OrderByDescending(spec.OrderByDescending);

        //now we need to create a seperate select query based on the first query
        var selectQuery = query as IQueryable<TResult>;

        //make sure it is not null and if it isnt then set the new query to be the original query with the select filter from the spec
        if (spec.Select != null) selectQuery = query.Select(spec.Select);

        //check if the spec is distinct is set if it is then make the selectquery distinct
        if (spec.IsDistinct) selectQuery = selectQuery?.Distinct();

        //now return the selectquery we modified unles its null then return the original query casted to a TResult
        return selectQuery ?? query.Cast<TResult>();
    }
}

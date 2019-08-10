using Horeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Horeb.Persistency
{
    public interface IReadOnlyWorkspace : IDisposable
    {        
        IEnumerable<string> Distinct<T>(Expression<Func<T, string>> expression) where T : class;
        IEnumerable<string> Distinct<T>(Expression<Func<T, string>> expression, Expression<Func<T, bool>> prediction) where T : class;       
        IEnumerable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> expression, Expression<Func<TSource, bool>> predictate, params Expression<Func<TSource, object>>[] includes) where TSource : class;
        int Count<T>(Expression<Func<T, bool>> predictate) where T : class;
        decimal Sum<T>(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predictate) where T : class;
        IQueryable<T> Queryable<T>() where T : class;
        bool Any<T>() where T : class;
        bool Exists<T>(Expression<Func<T, bool>> expression) where T: class, IValueClass<string>;
    }
}

using Horeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Horeb.Persistency
{
    public interface IWorkspaceContext : IDisposable
    {
        void Delete<T>(Expression<Func<T, bool>> expression) where T : class;
        void Delete<T>(T item) where T : class;        

        IEnumerable<T> All<T>() where T : class;
        IEnumerable<T> Where<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes) where T : class;

        void Add<T>(T item) where T : class, IValueClass<string>;
        void Update<T>(T item) where T : class;
        int Count<T>() where T : class;
    }
}

using Horeb.Persistency;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Horeb.Infrastructure.Data;

namespace Horeb.Pesistency.Workspace.File
{
    public class FileWorkspaceAdapter : IWorkspaceContext, IReadOnlyWorkspace
    {
        private readonly string _fileName;
        private readonly LiteDatabase _dbContext;

        public FileWorkspaceAdapter() : this(GetDefaultFileName())
        {
        }

        public FileWorkspaceAdapter(string filename)
        {
            _fileName = filename;
            _dbContext = new LiteDatabase(_fileName);
        }

        private static string GetDefaultFileName()
        {
            return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + "\\HorebDB.db";
        }

        public void Add<T>(T item) where T : class, IValueClass<string>
        {
                var collection = _dbContext.GetCollection<T>(typeof(T).ToString());
                collection.Insert(new BsonValue(item.Id) ,item);            
        }

        public IEnumerable<T> All<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            using (var db = new LiteDatabase(_fileName))
            {
                var collection = db.GetCollection<T>(typeof(T).ToString());
                return collection.FindAll();
            }
        }

        public IEnumerable<T> All<T>(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes) where T : class
        {
            using (var db = new LiteDatabase(_fileName))
            {
                var collection = db.GetCollection<T>(typeof(T).ToString());
                return collection.Find(expression);
            }
        }

        public bool Any<T>() where T : class
        {
            return Count<T>() != 0;
        }

        public int Count<T>() where T : class
        {
            using (var db = new LiteDatabase(_fileName))
            {
                var collection = db.GetCollection<T>(typeof(T).ToString());
                return collection.Count();
            }
        }

        public int Count<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using (var db = new LiteDatabase(_fileName))
            {
                var collection = db.GetCollection<T>(typeof(T).ToString());
                return collection.Count(predicate);
            }
        }

        public void Delete<T>(Expression<Func<T, bool>> expression) where T : class
        {
            using (var db = new LiteDatabase(_fileName))
            {
                var collection = db.GetCollection<T>(typeof(T).ToString());
                collection.Delete(expression);
            }
        }

        public void Delete<T>(T item) where T : class
        {
            using (var db = new LiteDatabase(_fileName))
            {
                var collection = db.GetCollection<T>(typeof(T).ToString());
                collection.Delete(x => x == item);
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IEnumerable<string> Distinct<T>(Expression<Func<T, string>> expression) where T : class
        {
            using (var db = new LiteDatabase(_fileName))
            {
                var collection = db.GetCollection<T>(typeof(T).ToString());
                return collection.FindAll().Select(expression.Compile()).Distinct().Where(x => !string.IsNullOrEmpty(x)).ToList();
            }            
        }

        public IEnumerable<string> Distinct<T>(Expression<Func<T, string>> expression, Expression<Func<T, bool>> prediction) where T : class
        {
            using (var db = new LiteDatabase(_fileName))
            {
                var collection = db.GetCollection<T>(typeof(T).ToString());
                return collection.FindAll().Where(prediction.Compile()).Select(expression.Compile()).Distinct().Where(x => !string.IsNullOrEmpty(x)).ToList();
            }            
        }

        public IQueryable<T> Queryable<T>() where T : class
        {
            return All<T>().AsQueryable();
        }

        public IEnumerable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> expression, Expression<Func<TSource, bool>> predictate, params Expression<Func<TSource, object>>[] includes) where TSource : class
        {
            if (predictate != null)
                return All<TSource>().Where(predictate.Compile()).Select(expression.Compile());
            return All<TSource>().Select(expression.Compile());
        }

        public decimal Sum<T>(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predictate) where T : class
        {
            if (predictate != null)
                return All<T>().Where(predictate.Compile()).Sum(selector.Compile());
            return All<T>().Sum(selector.Compile());
        }

        public void Update<T>(T item) where T : class
        {
            var collection = _dbContext.GetCollection<T>(typeof(T).ToString());
            collection.Update(item);
        }

        bool IReadOnlyWorkspace.Exists<T>(Expression<Func<T, bool>> expression)
        {
            var collection = _dbContext.GetCollection<T>(typeof(T).ToString());
            return collection.Exists(expression);
        }
    }
}

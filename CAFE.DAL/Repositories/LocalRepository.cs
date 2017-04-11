
using System;
using System.Collections.Generic;
using System.Linq;
using CAFE.DAL.Interfaces;

namespace CAFE.DAL.Repositories
{
    public sealed class LocalRepository<T> : IRepository<T> where T : class
    {
        private static readonly List<T> Data = new List<T>();

        public IEnumerable<TResult> Select<TResult>(Func<T, TResult> query)
        {
            return Data.Select(query);
        }

        public T Insert(T item)
        {
            Data.Add(item);
            return item;
        }
        public void InsertCollection(List<T> item)
        {
            Data.AddRange(item);
        }

        public T Update(T item)
        {
            return item;
        }

        public void Delete(T item)
        {
            Data.Remove(item);
        }

        public T Find(System.Linq.Expressions.Expression<Func<T, bool>> match)
        {
            return Data.FirstOrDefault(match.Compile());
        }

        public T FindWithDetaching(System.Linq.Expressions.Expression<Func<T, bool>> match)
        {
            return Data.FirstOrDefault(match.Compile());
        }
        
        public T FindLast(System.Linq.Expressions.Expression<Func<T, bool>> match)
        {
            return Data.LastOrDefault(match.Compile());
        }

        public IEnumerable<T> FindCollection(System.Linq.Expressions.Expression<Func<T, bool>> match)
        {
            return Data.Where(match.Compile());
        }

        public IQueryable<T> Select()
        {
            return Data.AsQueryable();
        }

        public IQueryable<T> SelectNoTracking()
        {
            return Data.AsQueryable();
        }

        public void DeleteThroughState(T item)
        {
            Data.Remove(item);
        }

        public void Clear()
        {
            Data.Clear();
        }

        public IQueryable<T> SqlQuery(string query)
        {
            return Data.AsQueryable();
        }

        public void TurnOffProxy()
        {
        }

        public IEnumerable<T> RunScriptWithReturnResult(string script, params object[] parameters)
        {
            return null;
        }
    }
}

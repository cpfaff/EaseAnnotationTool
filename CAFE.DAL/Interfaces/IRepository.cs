using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace CAFE.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<TResult> Select<TResult>(Func<T, TResult> query);        
        IQueryable<T> Select();
        IQueryable<T> SelectNoTracking();
        T Insert(T item);
        void InsertCollection(List<T> item);
        T Update(T item);
        void Delete(T item);
        void DeleteThroughState(T item);
        void Clear();
        T Find(Expression<Func<T, bool>> match);
        T FindWithDetaching(Expression<Func<T, bool>> match);
        T FindLast(Expression<Func<T, bool>> match);
        IEnumerable<T> FindCollection(Expression<Func<T, bool>> match);
        IQueryable<T> SqlQuery(string query);
        void TurnOffProxy();
        IEnumerable<T> RunScriptWithReturnResult(string script, params object[] parameters);
    }
}

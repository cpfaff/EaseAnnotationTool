using CAFE.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace CAFE.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : Models.DbBase
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            //this.Context = new ApplicationDbContext();
            this.Context = context;
        }

        public virtual T Insert(T item)
        {
            Context.Set<T>().Add(item);
            //Context.SaveChanges();
            SafeSaveContext();

            return item;
        }

        public virtual void InsertCollection(List<T> items)
        {
            Context.Set<T>().AddRange(items);
            //Context.SaveChanges();
            SafeSaveContext();
        }

        public virtual T Update(T item)
        {
            if (EntityState.Detached == Context.Entry(item).State)
                Context.Set<T>().Attach(item);

            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
                property.GetValue(item, null);

            Context.Entry(item).State = EntityState.Modified;
            //Context.SaveChanges();
            SafeSaveContext();
            return item;
        }

        public virtual void Delete(T item)
        {
            Context.Set<T>().Remove(item);
            //Context.SaveChanges();
            SafeSaveContext();
        }
        public virtual void DeleteThroughState(T item)
        {
            Context.Entry(item).State = EntityState.Deleted;
            //Context.SaveChanges();
            SafeSaveContext();
        }
        public virtual T Find(System.Linq.Expressions.Expression<Func<T, bool>> match)
        {
            var value = Context.Set<T>().FirstOrDefault(match);
            //Context.Entry(value).State = EntityState.Detached;
            return value;
        }

        public virtual T FindWithDetaching(System.Linq.Expressions.Expression<Func<T, bool>> match)
        {
            var value = Context.Set<T>().FirstOrDefault(match);
            Context.Entry(value).State = EntityState.Detached;
            return value;
        }

        public virtual T FindLast(System.Linq.Expressions.Expression<Func<T, bool>> match)
        {
            return Context.Set<T>().LastOrDefault(match);
        }

        public virtual IEnumerable<T> FindCollection(System.Linq.Expressions.Expression<Func<T, bool>> match)
        {
            var list = Context.Set<T>().Where(match);
            return list;
        }

        public virtual IEnumerable<TResult> Select<TResult>(Func<T, TResult> query)
        {
            return Context.Set<T>().Select(query);
        }
        public IQueryable<T> Select()
        {
            return Context.Set<T>();
        }
        public IQueryable<T> SelectNoTracking()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public void Clear()
        {
            Context.Set<T>().RemoveRange(Context.Set<T>().ToList());
            SafeSaveContext();
        }

        public IQueryable<T> SqlQuery(string query)
        {
            return Context.Set<T>().SqlQuery(query).AsQueryable();
        }

        public void TurnOffProxy()
        {
            Context.Configuration.ProxyCreationEnabled = false;
            Context.Configuration.LazyLoadingEnabled = false;
        }

        private void SafeSaveContext()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                ((IObjectContextAdapter)Context).ObjectContext.Refresh(RefreshMode.ClientWins, Context.Set<T>());
                Context.SaveChanges();
            }
        }

        public IEnumerable<T> RunScriptWithReturnResult(string script, params object[] parameters)
        {
            IEnumerable<T> result = default(IEnumerable<T>);

            try
            {
                var query = Context.Database.SqlQuery<T>(script, parameters);
                result = query.AsEnumerable();
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}

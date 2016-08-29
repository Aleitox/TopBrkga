using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Main.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext Context;

        protected DbSet<T> DbSet { get { return Context.Set<T>(); } }

        public Repository(DbContext dataContext)
        {
            Context = dataContext;
        }

        #region IRepository<T> Members

        public void Insert(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        #endregion
    }
}

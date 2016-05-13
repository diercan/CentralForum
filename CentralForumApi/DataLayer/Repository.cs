using System;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models.Models;
using Models.Context;


namespace DataLayer
{
    public class Repository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        internal Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> q = _dbSet;

            if (filter != null)
                q = q.Where(filter);

            if (!String.IsNullOrWhiteSpace(includeProperties))
                q = includeProperties.Split(new[] { ';' }).Aggregate(q, (current, property) => current.Include(property));

            return q.ToList();
        }

        public virtual IQueryable<T> GetQ()
        {
            return _dbSet;
        }

        public virtual T Get(object id)
        {
            return _dbSet.Find(id);
        }


        public virtual bool Contains(Expression<Func<T, bool>> filter)
        {
            var list = Get(filter);
            return list != null && list.Count() != 0;
        }

        public virtual void Insert(T entity, bool save = true)
        {
            _dbSet.Add(entity);
            if (save)
                _dbContext.SaveChanges();
        }

        public virtual bool TryInsert(T entity)
        {
            try
            {
                _dbSet.Add(entity);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Delete(object id, bool save = true)
        {
            var entity = _dbSet.Find(id);
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);

            if (save)
                _dbContext.SaveChanges();
        }

        public void Delete(T entity, bool save = true)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);

            if (save)
                _dbContext.SaveChanges();
        }


        public virtual bool TryDelete(object id)
        {
            try
            {
                return TryDelete(_dbSet.Find(id));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual bool TryDelete(T entity)
        {
            if (entity == null)
                return false;

            try
            {
                if (_dbContext.Entry(entity).State == EntityState.Detached)
                    _dbSet.Attach(entity);
                _dbSet.Remove(entity);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void AddOrUpdate(T entity, bool save = true)
        {
            _dbSet.AddOrUpdate(entity);
            if (save)
                _dbContext.SaveChanges();
        }
    }
}

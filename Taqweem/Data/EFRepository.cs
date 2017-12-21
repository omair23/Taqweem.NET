using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Taqweem.Data
{
    public class EFRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationDbContext Context { get { return _context; } }

        public EFRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TEntity Add<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                return item;
            }

            _context.Set<TEntity>().Add(item);
            _context.SaveChanges();

            return item;
        }

        public IEnumerable<TEntity> AddMultiple<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            if (items == null || !items.Any())
            {
                return items;
            }

            _context.Set<TEntity>().AddRange(items);
            _context.SaveChanges();

            return items;
        }

        public bool Delete<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                return false;
            }

            _context.Set<TEntity>().Attach(item);
            _context.Set<TEntity>().Remove(item);
            _context.SaveChanges();

            return true;
        }

        public bool DeleteMultiple<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            if (items == null || !items.Any())
            {
                return false;
            }

            //_context.Set<TEntity>().AttachRange(items);
            _context.Set<TEntity>().RemoveRange(items);
            _context.SaveChanges();

            return true;
        }

        public bool Update<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
            {
                return false;
            }

            _context.Set<TEntity>().Attach(item);
            //_context.Set<TEntity>().Update(item);
            _context.Entry(item).State = EntityState.Modified;
            
            _context.SaveChanges();

            return true;
        }

        public bool UpdateMultiple<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            if (items == null || !items.Any())
            {
                return false;
            }

            foreach (var item in items)
            {
                Update(item);
            }

            //_context.Set<TEntity>().AttachRange(items);
            //_context.Set<TEntity>().UpdateRange(items);

            //_context.SaveChanges();

            return true;
        }

        public TEntity GetByID<TEntity>(Expression<Func<TEntity, bool>> id) where TEntity : class
        {
            return _context.Set<TEntity>().FirstOrDefault(id);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            return _context.Set<TEntity>().OrderBy(orderBy);
        }

        public IQueryable<TEntity> GetAll<TEntity>(int pageIndex, int pageSize) where TEntity : class
        {
            return _context.Set<TEntity>().Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<TEntity> GetAll<TEntity, TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            return _context.Set<TEntity>().Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderBy(orderBy);
        }

        public IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> Find<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            return _context.Set<TEntity>().Where(predicate).Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderBy(orderBy);
        }
    }

}

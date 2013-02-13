using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BoilerMVC.Common;

namespace BoilerMVC.Data
{
    public class EntityRepository<T> : IRepository<T> where T : class
    {
        private DbContext _context;
        private DbSet<T> _table;

        public EntityRepository(IUnitOfWork context)
        {
            _context = context as DbContext;

            if (_context == null)
            {
                throw new Exception("Supplied IUnitOfWork is not a type of DbContext");
            }

            _table = _context.Set<T>();
        }

        public T Get(int id)
        {
            return _table.Find(id);
        }

        public IQueryable<T> All()
        {
            return _table;
        }

        public T First(Expression<Func<T, bool>> where)
        {
            return _table.FirstOrDefault(where);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return _table.Where<T>(where);
        }

        public void Add(T entity)
        {
            _table.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = Get(id);
            _table.Remove(entity);
        }
    }
}
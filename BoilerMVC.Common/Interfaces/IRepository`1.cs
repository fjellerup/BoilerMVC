using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BoilerMVC.Common
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Returns an entity based on its primary key integer value.
        /// </summary>
        /// <param name="id">Integer value of the entity's primary key column.</param>
        T Get(int id);

        /// <summary>
        /// Returns all the entities in the table.
        /// </summary>
        IQueryable<T> All();

        /// <summary>
        /// Returns the first entity that matches a specific condition.
        /// </summary>
        /// <param name="where">Predicate function to test each entity.</param>
        T First(Expression<Func<T, bool>> where);

        /// <summary>
        /// Returns entities that matches a specific condition.
        /// </summary>
        /// <param name="where">Predicate function to test each entity.</param>
        IQueryable<T> Where(Expression<Func<T, bool>> where);

        /// <summary>
        /// Adds an entity to the data context for database insertion.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        void Add(T entity);

        /// <summary>
        /// Deletes an entity from the database based on integer value of the primary key column.
        /// </summary>
        /// <param name="id">Integer value of the entity's primary key column.</param>
        void Delete(int id);
    }
}
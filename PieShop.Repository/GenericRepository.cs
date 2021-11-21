using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PieShop.Repository
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable where TEntity : class
    {
        #region Fields

        private readonly DbContext _context;

        private DbSet<TEntity> _entities;

        #endregion

        #region Ctor

        public GenericRepository(DbContext context)
        {
            this._context = context;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Rollback of entity changes and return full error message
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Error message</returns>
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry => entry.State = EntityState.Unchanged);
            }

            _context.SaveChanges();
            return exception.ToString();
        }

        #endregion

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();

                return _entities;
            }
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            try
            {
                Entities.Add(entity);
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            try
            {
                await Entities.AddAsync(entity);
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.AddRange(entities);
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                await Entities.AddRangeAsync(entities);
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual void Update(TEntity entity, params object[] avoidProperties)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            try
            {
                AvoidProperties(Entities.Update(entity), avoidProperties.ToList());
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, params object[] avoidProperties)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            try
            {
                if (avoidProperties.Length > 0)
                {
                    foreach (var entity in entities)
                    {
                        AvoidProperties(Entities.Update(entity), avoidProperties.ToList());
                    }
                }
                else
                {
                    Entities.UpdateRange(entities);
                }
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual void UpdateWithAcceptedProperties(TEntity entity, params object[] acceptedProperties)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            try
            {
                AcceptedPropertyToModify(Entities.Update(entity), acceptedProperties.ToList());
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual void UpdateRangeWithAcceptedProperties(IEnumerable<TEntity> entities, params object[] acceptedProperties)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            try
            {
                if (acceptedProperties.Length > 0)
                {
                    foreach (var entity in entities)
                    {
                        AcceptedPropertyToModify(Entities.Update(entity), acceptedProperties.ToList());
                    }
                }
                else
                {
                    Entities.UpdateRange(entities);
                }
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }
        public virtual void AvoidProperties(EntityEntry entry, List<object> avoidProperties)
        {
            if (avoidProperties != null)
            {
                var properties = entry.Entity.GetType().GetProperties()
                    .Where(prop => !Attribute.IsDefined(prop, typeof(NotMappedAttribute)));
                foreach (var p in properties)
                {
                    if (avoidProperties.Any(x => x.ToString() == p.Name))
                    {
                        if (IsSimpleType(p.PropertyType))
                        {
                            entry.Property(p.Name).IsModified = false;
                        }
                    }
                }

                //foreach (var item in avoidProperties)
                //{
                //    foreach (PropertyInfo p in item.GetType().GetProperties())
                //        entry.Property(p.Name).IsModified = false;
                //}
            }
        }

        private void AcceptedPropertyToModify(EntityEntry entry, List<object> acceptedProperties)
        {
            if (acceptedProperties != null)
            {
                var properties = entry.Entity.GetType().GetProperties()
                    .Where(prop => !Attribute.IsDefined(prop, typeof(NotMappedAttribute)));
                foreach (var p in properties)
                {
                    if (!acceptedProperties.Any(x => x.ToString() == p.Name))
                    {
                        if (IsSimpleType(p.PropertyType))
                        {
                            entry.Property(p.Name).IsModified = false;
                        }
                    }
                }
            }
        }

        private static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
            typeof(Enum),
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }


        public virtual int SaveChanges()
        {
            int value = _context.SaveChanges();
            foreach (var entry in _context.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }
            return value;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            int value = await _context.SaveChangesAsync();
            foreach (var entry in _context.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }
            return value;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.RemoveRange(entities);
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual int Count(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
            {
                return Entities.Count();
            }
            else
            {
                return Entities.Count(expression);
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
            {
                return await Entities.CountAsync();
            }
            else
            {
                return await Entities.CountAsync(expression);
            }
        }

        public virtual bool IsExist(Expression<Func<TEntity, bool>> expression) => Entities.Any(expression);
        public virtual async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression)
            => await Entities.AnyAsync(expression);

        public virtual TEntity Details(Expression<Func<TEntity, bool>> expression) => Entities.FirstOrDefault(expression);
        public virtual async Task<TEntity> DetailsAsync(Expression<Func<TEntity, bool>> expression)
            => await Entities.FirstOrDefaultAsync(expression);

        public virtual TEntity GetById(object id) => Entities.Find(id);
        public virtual async Task<TEntity> GetByIdAsync(object id) => await Entities.FindAsync(id);

        public virtual IEnumerable<TEntity> Get() => Entities.AsEnumerable();
        public virtual async Task<IEnumerable<TEntity>> GetAsync() => await Entities.ToListAsync();

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
            => Entities.Where(expression).AsEnumerable();

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
            => await Entities.Where(expression).ToListAsync();

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int top = 200, bool disableTracking = true)
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int top = 200, bool disableTracking = true)
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual IEnumerable<TResult> Get<TResult>(Expression<Func<TEntity, TResult>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int top = 200, bool disableTracking = true) where TResult : class
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToList();
            }
            else
            {
                return query.Select(selector).ToList();
            }
        }

        public virtual async Task<IEnumerable<TResult>> GetAsync<TResult>(Expression<Func<TEntity, TResult>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int top = 200, bool disableTracking = true) where TResult : class
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(selector).ToListAsync();
            }
            else
            {
                return await query.Select(selector).ToListAsync();
            }
        }


        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }
            else
            {
                return query.FirstOrDefault();
            }
        }

        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }

        public virtual TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity,
                object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).FirstOrDefault();
            }
            else
            {
                return query.Select(selector).FirstOrDefault();
            }
        }

        public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity,
                object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(selector).FirstOrDefaultAsync();
            }
            else
            {
                return await query.Select(selector).FirstOrDefaultAsync();
            }
        }

        public virtual async Task<TResult> GetLastOrDefaultAsync<TResult>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Expression<Func<TEntity, TResult>> selector = null,
                  Expression<Func<TEntity, bool>> predicate = null,
                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                return await orderBy(query).Select(selector).LastOrDefaultAsync();
            }
            else
            {
                return await query.Select(selector).LastOrDefaultAsync();
            }
        }

        public virtual async Task<TEntity> GetLastOrDefaultAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                  Expression<Func<TEntity, bool>> predicate = null,
                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity,
                      object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = Entities;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).LastOrDefaultAsync();
            }
            else
            {
                return await query.LastOrDefaultAsync();
            }
        }

        private bool _disposed = false;
        /// <summary>
        /// Dispose the dbContext after finish task
        /// </summary>
        /// <param name="disposing">Flag for indicating desposing or not</param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Dispose the Context after finishing task
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

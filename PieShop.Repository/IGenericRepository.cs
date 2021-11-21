using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PieShop.Repository
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }

        #endregion

        #region Methods
        bool IsExist(Expression<Func<TEntity, bool>> expression);
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression);

        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);

        void InsertRange(IEnumerable<TEntity> entities);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="avoidProperties">Avoid those properties which are not required to update</param>
        void Update(TEntity entity, params object[] avoidProperties);

        /// <summary>
        /// Update Multiple entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void UpdateRange(IEnumerable<TEntity> entities, params object[] avoidProperties);
        void UpdateWithAcceptedProperties(TEntity entity, params object[] acceptedProperties);
        void UpdateRangeWithAcceptedProperties(IEnumerable<TEntity> entities, params object[] acceptedProperties);
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete Multiple entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void DeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Avoid those property which are not required to update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="avoidProperties"></param>
        /// <returns></returns>
        void AvoidProperties(EntityEntry entry, List<object> avoidProperties);

        int SaveChanges();
        Task<int> SaveChangesAsync();

        IEnumerable<TEntity> Get();
        Task<IEnumerable<TEntity>> GetAsync();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                        int top = 200,
                                        bool disableTracking = true);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                        int top = 200,
                                        bool disableTracking = true);

        Task<IEnumerable<TResult>> GetAsync<TResult>(Expression<Func<TEntity, TResult>> selector = null,
                                        Expression<Func<TEntity, bool>> predicate = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                        int top = 200,
                                        bool disableTracking = true) where TResult : class;


        TEntity Details(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> DetailsAsync(Expression<Func<TEntity, bool>> expression);

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        Task<TEntity> GetLastOrDefaultAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                  Expression<Func<TEntity, bool>> predicate = null,
                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true);
        Task<TResult> GetLastOrDefaultAsync<TResult>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Expression<Func<TEntity, TResult>> selector = null,
                  Expression<Func<TEntity, bool>> predicate = null,
                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true);

        int Count(Expression<Func<TEntity, bool>> expression = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression = null);
        #endregion
    }
}

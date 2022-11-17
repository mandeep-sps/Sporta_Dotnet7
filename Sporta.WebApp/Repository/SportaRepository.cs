using Microsoft.EntityFrameworkCore;
using Sporta.Data.Database.Sporta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sporta.WebApp.Repository
{
    /// <summary>
    /// Sporta Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SportaRepository<TEntity> : ISportaRepository<TEntity> where TEntity : class, ISportaAudit
    {
        private readonly Sporta_DbContext _context;
        private readonly DbSet<TEntity> _set;
        public SportaRepository(Sporta_DbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }


        /// <summary>
        /// Save all the changes to database ***Mandatory to call this function after every transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        #region Get Methods

        /// <summary>
        ///  Get all data list of entity.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _set
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Get all data list of entity based on given condition.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _set
                .Where(x => !x.IsDeleted)
                .Where(filter)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Get all data list of entity based on given condition including joint properties.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var nonDeleted = _set
                .Where(x => !x.IsDeleted)
                .Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                    await nonDeleted.Include(property).LoadAsync();
            }

            return await nonDeleted.ToListAsync();

        }

        /// <summary>
        /// Get all data list of entity including joint properties.
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var nonDeleted = _set
                 .Where(x => !x.IsDeleted);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                    await nonDeleted.Include(property).LoadAsync();
            }

            return await nonDeleted.ToListAsync();
        }


        /// <summary>
        /// Get Entity based on given condition
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _set.Where(x => !x.IsDeleted).FirstOrDefaultAsync(filter);
        }

        /// <summary>
        /// Get Entity based on given condition including joint properties.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var nonDeleted = _set.Where(x => !x.IsDeleted);
            foreach (var property in includeProperties)
                await nonDeleted.Include(property).LoadAsync();

            return await nonDeleted.FirstOrDefaultAsync(filter);
        }

        #endregion


        #region Add Methods

        /// <summary>
        /// Add new entry
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="createdby"></param>
        /// <returns></returns>
        public async Task AddAsync(TEntity entity, int createdby)
        {
            entity.CreatedBy = createdby;
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedBy = createdby;
            entity.ModifiedOn = DateTime.Now;
            await _set.AddAsync(entity);
        }

        /// <summary>
        /// Add new batch entry
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _set.AddRangeAsync(entities);
        }

        #endregion


        #region Update Methods

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="modifiedBy"></param>
        public void Update(TEntity entity, int modifiedBy)
        {
            entity.ModifiedBy = modifiedBy;
            entity.ModifiedOn = DateTime.Now;

            _set.Update(entity);
        }

        /// <summary>
        /// Update batch at once
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _set.UpdateRange(entities);
        }

        #endregion


        #region Delete Methods

        /// <summary>
        ///  Remove entry from database permanently
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Delete(TEntity entity)
        {
            _set.Remove(entity);
        }

        /// <summary>
        /// Remove batch from database permanently
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _set.RemoveRange(entities);
        }


        /// <summary>
        /// Archive entry from database (Set IsDeleted = 1 or transfer to archive table)
        /// </summary>
        /// <param name="modifiedBy"></param>
        /// <param name="id"></param>
        public async Task Remove(object id, int modifiedBy)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.ModifiedBy = modifiedBy;
                entity.ModifiedOn = DateTime.Now;

                _set.Update(entity);
            }
        }


        /// <summary>
        /// Archive batch from database (Set IsDeleted = 1 or transfer to archive table)
        /// </summary>
        /// <param name="modifiedBy"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task RemoveRange(Expression<Func<TEntity, bool>> filter, int modifiedBy)
        {
            var entities = await _set.Where(filter).ToListAsync();
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                    Update(entity, modifiedBy);
            }

        }

        #endregion


        #region Extensions or Miscellaneous Methods
        /// <summary>
        /// To check whether entity exists or not based on (optional) filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await _set.Where(x => !x.IsDeleted).AnyAsync(filter);
        }

        /// <summary>
        /// To get the data count of entity present based on (optional) filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await _set.Where(x => !x.IsDeleted).CountAsync(filter);
        }

        #endregion
    }

    /// <summary>
    /// Sporta Repository V2
    /// </summary>
    public class SportaRepositoryV2 : ISportaRepositoryV2
    {
        private readonly Sporta_DbContext _context;
        public SportaRepositoryV2(Sporta_DbContext context)
        {
            _context = context;
        }

        #region Get Method

        /// <summary>
        /// Get All Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get All Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            return await _context.Set<T>().Where(filter).AsNoTracking().ToListAsync();
        }

        /// <summary>
        ///  Get All Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            var query = _context.Set<T>().Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                    await query.Include(property).LoadAsync();
            }
            return await query.ToListAsync();
        }

        /// <summary>
        ///  Get All Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            var query = _context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                    await query.Include(property).LoadAsync();
            }

            return await query.ToListAsync();
        }

        #endregion


        #region Delete Method

        /// <summary>
        /// Delete Range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Remove Range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="modifiedBy"></param>
        public void RemoveRange<T>(IEnumerable<T> entities, int modifiedBy) where T : class, ISportaAudit
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.ModifiedBy = modifiedBy;
                entity.ModifiedOn = DateTime.Now;
            }

            _context.Set<T>().UpdateRange(entities.ToList());
        }


        /// <summary>
        /// Archive entry from database (Set IsDeleted = 1 or transfer to archive table)
        /// </summary>
        /// <param name="modifiedBy"></param>
        /// <param name="id"></param>

        public async Task Remove<T>(object id, int modifiedBy) where T : class, ISportaAudit
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.ModifiedBy = modifiedBy;
                entity.ModifiedOn = DateTime.Now;

                _context.Set<T>().Update(entity);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task Update<T>(object id, int modifiedBy) where T : class, ISportaAudit
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                entity.ModifiedBy = modifiedBy;
                entity.ModifiedOn = DateTime.Now;

                _context.Set<T>().Update(entity);
            }
        }

        #endregion

        /// <summary>
        /// Save all the changes to database ***Mandatory to call this function after every transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}

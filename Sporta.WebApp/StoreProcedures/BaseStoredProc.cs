using Microsoft.EntityFrameworkCore;
using Sporta.Data.CustomSql;
using Sporta.WebApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sporta.WebApp.StoreProcedures
{
    /// <summary>
    /// Base Stored Proc
    /// </summary>
    public class BaseStoredProc : IBaseStoredProc
    {
        public readonly StoredProcedureClass _spContext;
        public BaseStoredProc(StoredProcedureClass spContext)
        {
            _spContext = spContext;
        }

        public async Task<ICollection<TEntity>> ExecuteStoredProcActiveExecutiveDetailsAsync<TEntity>(string storedProcedure) where TEntity : class
        {
            return await _spContext.Set<TEntity>()
               .FromSqlRaw(storedProcedure)
               .AsNoTracking()
               .ToListAsync();
        }

        /// <summary>
        /// Execute Stored Proc Drive Executives Details Async
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<ICollection<TEntity>> ExecuteStoredProcDriveExecutivesDetailsAsync<TEntity>(string storedProcedure, params object[] parameters) where TEntity : class
        {
            return await _spContext.Set<TEntity>()
               .FromSqlRaw(storedProcedure.ExtendParameters(parameters.Length), parameters)
               .AsNoTracking()
               .ToListAsync();
        }

        /// <summary>
        /// Execute Stored Proc Async
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<int> ExecuteStoredProcAsync(string storedProcedure, params object[] parameters)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Execute Stored Proc Async
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> ExecuteStoredProcAsync<TEntity>(string storedProcedure, params object[] parameters) where TEntity : class
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await Task.Run(() =>
            {
                return _spContext.Set<TEntity>()
                    .FromSqlRaw(storedProcedure.ExtendParameters(parameters.Length), parameters)
                    .AsNoTracking()
                    .AsEnumerable()
                    .FirstOrDefault();
            });
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Execute Stored Proc Collection Async
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<ICollection<TEntity>> ExecuteStoredProcCollectionAsync<TEntity>(string storedProcedure, params object[] parameters) where TEntity : class
        {
            return await _spContext.Set<TEntity>()
               .FromSqlRaw(storedProcedure.ExtendParameters(parameters.Length), parameters)
               .AsNoTracking()
               .ToListAsync();
        }

        /// <summary>
        /// Execute Stored Proc With ID Async
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<int> ExecuteStoredProcWithIDAsync(string storedProcedure, params object[] parameters)
        {
            throw new NotImplementedException();

        }
    }
}

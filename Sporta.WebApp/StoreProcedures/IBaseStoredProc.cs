using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporta.WebApp.StoreProcedures
{
    /// <summary>
    /// Base Stored Proc Interface
    /// </summary>
    public interface IBaseStoredProc
    {
        /// <summary>
        /// Execute Stored Proc Async
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<int> ExecuteStoredProcAsync
            (string storedProcedure, params object[] parameters);

        /// <summary>
        /// Execute Stored Proc With ID Async
        /// </summary>
        public Task<int> ExecuteStoredProcWithIDAsync
            (string storedProcedure, params object[] parameters);

        /// <summary>
        /// Execute Stored Proc Async
        /// </summary>
        public Task<TEntity> ExecuteStoredProcAsync<TEntity>
            (string storedProcedure, params object[] parameters) where TEntity : class;

        /// <summary>
        /// Execute Stored Proc Collection Async
        /// </summary>
        public Task<ICollection<TEntity>> ExecuteStoredProcCollectionAsync<TEntity>
            (string storedProcedure, params object[] parameters) where TEntity : class;

        public Task<ICollection<TEntity>> ExecuteStoredProcDriveExecutivesDetailsAsync<TEntity>
            (string storedProcedure, params object[] parameters) where TEntity : class;

        public Task<ICollection<TEntity>> ExecuteStoredProcActiveExecutiveDetailsAsync<TEntity>
            (string storedProcedure) where TEntity : class;
    }
}

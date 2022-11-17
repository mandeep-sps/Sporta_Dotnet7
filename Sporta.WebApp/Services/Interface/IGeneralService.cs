using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporta.WebApp.Services.Interface
{
    /// <summary>
    /// General Service Interface
    /// </summary>
    public interface IGeneralService
    {
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<DropdownModel>>> GetList(ListType type, object param = null);

        /// <summary>
        /// Create Error Log
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>Error ID</returns>
        Task<ServiceResult<Guid>> CreateErrorLog(ErrorRequest errorRequest);

        /// <summary>
        /// Get Error By Id
        /// </summary>
        Task<ServiceResult<ErrorRequest>> GetErrorById(Guid errorId);

    }
}

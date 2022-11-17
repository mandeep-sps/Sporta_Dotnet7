using Sporta.Data.Models;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporta.WebApp.Services.Interface
{
    public interface IDashboardService
    {
        /// <summary>
        /// Get Total Drive Count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<int>> GetTotalDriveCount(int userId);

        /// <summary>
        /// Current Drive Count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<int>> CurrentLiveDrive(int userId);

        /// <summary>
        /// Upcoming Drive Count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<int>> UpcomingDrive(int userId);

        /// <summary>
        /// Total Candidates Count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<int>> TotalCandidatesEnrolled(int userId);

        /// <summary>
        /// Total Candidates Count By Date
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<GetCountByDate_Result>>> TotalCandidatesEnrolledByDate(int userId);

        /// <summary>
        /// Total Drive Count By Date
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<GetCountByDate_Result>>> TotalDriveCreatedByDate(int userId);

        /// <summary>
        /// Category percentage to show in bar graph
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<GetCategoryPercentBasedOnDrives_Result>>> CategoryPercentByDrives(int userId);

        /// <summary>
        /// drive created percentage to show in doughnut graph
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<GetDrivePercentByMonths_Result>>> DrivePercentPerMonth(int userId);
    }
}

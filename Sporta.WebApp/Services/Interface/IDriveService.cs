using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporta.WebApp.Services.Interface
{
    /// <summary>
    /// Drive Service Interface
    /// </summary>
    public interface IDriveService
    {
        /// <summary>
        /// Get Drives
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<DriveResponseModel>>> GetDrives(int userId, bool isLive, bool isUpcomming);

        /// <summary>
        /// Get Drive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<DriveResponseModel>> GetDrive(int id);

        /// <summary>
        /// Get Drive By Candidate
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        Task<ServiceResult<CandidateDriveReponseModel>> GetDriveByCandidate(int id);

        /// <summary>
        /// Create Drive
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        Task<ServiceResult<DriveResponseModel>> CreateDrive(DriveRequestModel request, int createdBy);

        /// <summary>
        /// Update Drive
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<DriveResponseModel>> UpdateDrive(DriveRequestModel request, int modifiedBy);

        /// <summary>
        /// Remove Drive
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> RemoveDrive(int id, int modifiedBy);

        /// <summary>
        /// Delete Drive Permanently
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> DeleteDrivePermanently(int id);

        /// <summary>
        /// Enable / Disable Drive
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> EnableDisableDrive(int id, bool status, int modifiedBy);

        /// <summary>
        /// Get Filtered Archived Drives
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<DriveResponseModel>>> GetFilteredArchivedDrives(DriveFilterRequestModel filters, int userId);

        /// <summary>
        /// Get Executive Details By Drive Id
        /// </summary>
        /// <param name="DriveId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<ApplicationUserModel>>> GetExecutiveDetailsByDriveId(int DriveId);
    }
}

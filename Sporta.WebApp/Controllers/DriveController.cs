using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Services.Interface;
using System.Threading.Tasks;

namespace Sporta.WebApp.Controllers
{
    /// <summary>
    /// Drive Controller
    /// </summary>
    public class DriveController : BaseController
    {
        private readonly IGeneralService _generalService;
        private readonly IDriveService _driveService;
        private readonly IResultService _reportService;
        public DriveController(IDriveService driveService, IGeneralService generalService, IResultService reportService)
        {
            _generalService = generalService;
            _driveService = driveService;
            _reportService = reportService;
        }

        #region Drive Views

        /// <summary>
        /// Index View
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1,2")]
        public IActionResult Index(bool isLive = false, bool isUpcomming = false)
        {
            ViewBag.IsLive = isLive;
            ViewBag.IsUpcomming = isUpcomming;
            return View();
        }
        #endregion

        #region Drive CRUD Actions

        /// <summary>
        /// Get Drives
        /// </summary>
        /// <returns></returns>      
        public async Task<IActionResult> GetDrives(bool isLive = false, bool isUpcomming = false)
        {
            return GetResult(await _driveService.GetDrives(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId(), isLive, isUpcomming));
        }
        /// <summary>
        /// Get Drive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetDrive(int id)
        {
            return GetResult(await _driveService.GetDrive(id));
        }

        /// <summary>
        /// Create Drive
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateDrive(DriveRequestModel request)
        {
            return GetResult(await _driveService.CreateDrive(request, User.GetUserId()));
        }

        /// <summary>
        /// Update Drive
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateDrive(DriveRequestModel request)
        {
            return GetResult(await _driveService.UpdateDrive(request, User.GetUserId()));
        }

        /// <summary>
        /// Remove Drive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RemoveDrive(int id)
        {
            return GetResult(await _driveService.RemoveDrive(id, User.GetUserId()));
        }

        /// <summary>
        /// Delete Drive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteDrive(int id)
        {
            return GetResult(await _driveService.DeleteDrivePermanently(id));
        }

        /// <summary>
        /// Enable / Disable Drive
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IActionResult> EnableDisableDrive(int id, bool status)
        {
            return GetResult(await _driveService.EnableDisableDrive(id, status, User.GetUserId()));
        }

        /// <summary>
        /// Filtered Archived Drives
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FilteredArchivedDrives(DriveFilterRequestModel filter)
        {
            return GetResult(await _driveService.GetFilteredArchivedDrives(filter, User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Get All Drives
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetAllDrives_Dropdown()
        {
            return GetResult(await _generalService.GetList(ListType.Drives));
        }

        /// <summary>
        /// Get Drives B yExecutive Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetDrivesByExecutive_Dropdown()
        {
            return GetResult(await _generalService.GetList(ListType.DrivesByExecutives, User.GetUserId()));
        }

        /// <summary>
        /// Get Categories Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetCategories_Dropdown()
        {
            return GetResult(await _generalService.GetList(ListType.Categories));
        }

        /// <summary>
        /// Get Executives Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetExecutives_Dropdown()
        {
            return User.GetUserRole() == (int)UserRole.Admin ? GetResult(await _generalService.GetList(ListType.User_Executives))
               : GetResult(await _generalService.GetList(ListType.Current_User_Executives, User.GetUserId()));
        }

        /// <summary>
        /// Get Archived Drives Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetArchivedDrives_Dropdown()
        {
            return User.GetUserRole() == (int)UserRole.Admin ? GetResult(await _generalService.GetList(ListType.ArchivedDrives))
                : GetResult(await _generalService.GetList(ListType.ArchivedDrivesByExecutives, User.GetUserId()));
        }

        /// <summary>
        /// Get Archived Drives By Executive Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetArchivedDrivesByExecutive_Dropdown()
        {
            return GetResult(await _generalService.GetList(ListType.ArchivedDrivesByExecutives, User.GetUserId()));
        }


        /// <summary>
        /// Get candidates Details By Drive Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDriveCandidatesDetails(int id)
        {
            return GetResult(await _reportService.GetReportByDrive(id));
        }

        public async Task<JsonResult> GetDriveExecuitvesDetails(int id)
        {
            return GetResult(await _driveService.GetExecutiveDetailsByDriveId(id));
        }


        #endregion
    }
}

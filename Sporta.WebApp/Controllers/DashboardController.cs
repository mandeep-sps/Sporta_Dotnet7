using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Services.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sporta.WebApp.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IGeneralService _generalService;
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService, IGeneralService generalService)
        {
            _generalService = generalService;
            _dashboardService = dashboardService;
        }


        /// <summary>
        /// Get total Drives Count
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetTotalDriveCount()
        {
            return GetResult(await _dashboardService.GetTotalDriveCount(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }

        /// <summary>
        /// Get Current Drives Count
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetLiveDriveCount()
        {
            return GetResult(await _dashboardService.CurrentLiveDrive(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }

        /// <summary>
        /// Get Upcoming Drives Count
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetUpcomingDriveCount()
        {
            return GetResult(await _dashboardService.UpcomingDrive(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }

        /// <summary>
        /// Get Total candidates Count
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetTotalCandidatesCount()
        {
            return GetResult(await _dashboardService.TotalCandidatesEnrolled(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }

        /// <summary>
        /// Get Total Candidates Count By Date
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetTotalCandidatesCountByDateTime()
        {
            return GetResult(await _dashboardService.TotalCandidatesEnrolledByDate(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }

        /// <summary>
        /// Get Total Drives Count By Date
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetTotalDriveCountByDateTime()
        {
            return GetResult(await _dashboardService.TotalDriveCreatedByDate(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }

        /// <summary>
        /// Get Percentage of Category based on Drives
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetCategoryPercent()
        {
            return GetResult(await _dashboardService.CategoryPercentByDrives(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }

        /// <summary>
        /// Get Percentage of Drives based on Months
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetDrivePercent()
        {
            return GetResult(await _dashboardService.DrivePercentPerMonth(User.GetUserRole() == (int)UserRole.Admin ? 0 : User.GetUserId()));
        }
    }
}

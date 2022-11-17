using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sporta.WebApp.Services.Interface;
using System.Threading.Tasks;

namespace Sporta.WebApp.Controllers
{
    /// <summary>
    /// Report Controller
    /// </summary>

    [Authorize(Roles = "1,2")]
    public class ReportController : BaseController
    {
        private readonly IResultService _reportService;

        public ReportController(IResultService reportService)
        {
            _reportService = reportService;
        }

        #region Report Views

        /// <summary>
        /// Index View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Index(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        #endregion


        #region Report CRUD Actions

        /// <summary>
        /// Get Report By Drive
        /// </summary>
        /// <param name="driveId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetReportBydrive(int driveId)
        {
            return GetResult(await _reportService.GetReportByDrive(driveId));
        }

        /// <summary>
        /// Get Result Detail By Id
        /// </summary>
        /// <param name="ResultId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetResultDetailById(int ResultId)
        {
            return GetResult(await _reportService.GetResultDetailById(ResultId));
        }

        /// <summary>
        /// Get Result Detail By DriveId
        /// </summary>
        /// <param name="driveId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetResultDetailByDriveId(int driveId)
        {
            return GetResult(await _reportService.GetResultDetailByDriveId(driveId));
        }
        #endregion

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Services.Interface;
using System.Threading.Tasks;

namespace Sporta.WebApp.Controllers
{
    /// <summary>
    /// Candidate Controller
    /// </summary>
    public class CandidateController : BaseController
    {
        private readonly ICandidateService _candidateService;
        private readonly IDriveService _driveService;
        private readonly IQuestionService _questionService;
        private readonly IResultService _resultService;

        public CandidateController(
            ICandidateService candidateService,
            IDriveService driveService,
            IResultService resultService,
            IQuestionService questionService)
        {
            _candidateService = candidateService;
            _driveService = driveService;
            _questionService = questionService;
            _resultService = resultService;
        }

        #region Candidate Views

        /// <summary>
        /// Index View
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "3")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Test View
        /// </summary>
        /// <param name="driveId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IActionResult Test(int driveId, int categoryId)
        {
            ViewData["DriveId"] = driveId;
            ViewData["CategoryId"] = categoryId;
            return View();
        }

        #endregion

        #region Candidate CRUD Actions

        /// <summary>
        /// Get All Candidates according to Drive
        /// </summary>
        /// <param name="driveId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCandidates(int driveId)
        {
            return GetResult(await _candidateService.GetCandidatesByDrive(driveId));
        }

        /// <summary>
        /// Update Candidate
        /// </summary>
        /// <param name="applicationUserRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UpdateCandidate(ApplicationUserRequestModel applicationUserRequest)
        {
            var response = await _candidateService.UpdateCandidate(applicationUserRequest, User.GetUserId());
            if (response.Data != null)
                return Json(new { isSuccess = true, data = response.Data, message = response.Message });
            else
                return Json(new { isSuccess = false, message = response.Message });
        }

        #endregion

        #region Test Screen Actions

        /// <summary>
        /// Get Drive By Candidate
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetDriveByCandidate()
        {
            return GetResult(await _driveService.GetDriveByCandidate(User.GetUserId()));

        }

        /// <summary>
        /// Get Drive Category Questions
        /// </summary>
        /// <param name="driveId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetDriveCategoryQuestions(int driveId, int categoryId)
        {
            return GetResult(await _questionService.GetDriveCategoryQuestions(driveId, categoryId, User.GetUserId()));
        }

        /// <summary>
        /// Save Test Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveTestCategory(TestSubmitRequestModel request)
        {
            await _resultService.UpdateCandidateResultDetail(request, User.GetUserId());
            /// need to opptimixe
            return Json(new { IsSuccess = true, Message = "Result Saved!" });
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Services.Interface;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sporta.WebApp.Controllers
{
    /// <summary>
    /// Application User Controller
    /// </summary>
    public class ApplicationUserController : BaseController
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IGeneralService _generalService;
        public ApplicationUserController(IApplicationUserService applicationUserService, IGeneralService generalService)
        {
            _applicationUserService = applicationUserService;
            _generalService = generalService;
        }

        #region Application User Views

        /// <summary>
        /// Index View
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// User Index View
        /// </summary>
        /// <returns></returns>
        public IActionResult UserIndex()
        {
            return View();
        }

        /// <summary>
        /// Candidates View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "1,2")]
        public IActionResult Candidates(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var loginResponse = await _applicationUserService.Signin(loginModel);
            if (loginResponse.Data != null)
            {
                if (string.IsNullOrEmpty(loginResponse.Data.ResponseType))
                {
                    var data = loginResponse.Data;
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(AppClaimTypes.UserId, data.UserId.ToString()));
                    identity.AddClaim(new Claim(AppClaimTypes.UserName, data.UserName));
                    identity.AddClaim(new Claim(AppClaimTypes.Role, data.RoleId.ToString()));
                    identity.AddClaim(new Claim(AppClaimTypes.Name, data.Name));
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (data.RoleId == (int)UserRole.Admin || data.RoleId == (int)UserRole.Executive)
                        return Json(new { isSuccess = true, url = "/ApplicationUser/UserIndex", message = loginResponse.Message });

                    else if (data.RoleId == (int)UserRole.Candidate)
                        return Json(new { isSuccess = true, url = "/Candidate/Index", message = loginResponse.Message, type = loginResponse.Data?.ResponseType });

                    else
                        return Json(new { isSuccess = true, url = "/Home/Index", message = loginResponse.Message });
                }
                else
                {
                    return Json(new { isSuccess = false, message = loginResponse.Message, type = loginResponse.Data.ResponseType });
                }
            }
            else
            {
                return Json(new { isSuccess = false, message = loginResponse.Message });
            }

        }


        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        #endregion

        #region Application User CRUD

        /// <summary>
        /// Get All Application Users
        /// </summary>
        /// <returns>Gets collection of users</returns>
        public async Task<JsonResult> GetAllUsers()
        {
            return GetResult(await _applicationUserService.GetExecutives());
        }

        /// <summary>
        /// Signup for Candidate
        /// </summary>
        /// <param name="applicationUserRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Signup(ApplicationUserRequestModel applicationUserRequest)
        {
            applicationUserRequest.ApplicationRoleId = (int)UserRole.Candidate;

            var response = await _applicationUserService.SignupUser(applicationUserRequest, 1);

            if (response.Data != null)
                return Json(new { isSuccess = true, url = "/ApplicationUser/Login", data = response.Data });
            else
                return Json(new { isSuccess = false, message = response.Message });
        }

        /// <summary>
        /// Get Email
        /// </summary>
        /// <returns></returns>
        public IActionResult GetEmail()
        {
            return Json(User.GetUsername());
        }


        /// <summary>
        /// Add Application User
        /// </summary>
        /// <param name="applicationUserRequest"></param>
        /// <returns>Created application user</returns>
        public async Task<JsonResult> AddUser(ApplicationUserRequestModel applicationUserRequest)
        {
            return GetResult(await _applicationUserService.AddExecutive(applicationUserRequest, User.GetUserId()));
        }

        /// <summary>
        /// Gets Application User Details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Application User Details based on ID</returns>
        public async Task<JsonResult> UserDetails(int userId, int roleId)
        {
            var response = await _applicationUserService.GetApplicationUser(userId, roleId);
            if (response.Data != null)
                return Json(new { isSuccess = true, data = response.Data, message = response.Message });
            else
                return Json(new { isSuccess = false, message = response.Message });
        }

        /// <summary>
        /// Update Application User
        /// </summary>
        /// <param name="applicationUserRequest"></param>
        /// <returns>Updated Application user</returns>
        public async Task<JsonResult> UpdateUser(ApplicationUserRequestModel applicationUserRequest)
        {
            return GetResult(await _applicationUserService.UpdateUser(applicationUserRequest, User.GetUserId()));
        }


        /// <summary>
        /// Remove Application User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True / False based on request</returns>
        public async Task<JsonResult> RemoveUser(int userId, int roleId)
        {
            return GetResult(await _applicationUserService.RemoveUser(userId, roleId, User.GetUserId()));
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> ChangePassword(string newPassword, string oldPassword)
        {
            return GetResult(await _applicationUserService.ChangePassword(newPassword, oldPassword, User.GetUserId()));
        }

        /// <summary>
        /// Get Branches
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetBranches()
        {
            return GetResult(await _generalService.GetList(ListType.Branches));
        }

        public async Task<JsonResult> GetAllActiveExecutives()
        {
            return GetResult(await _applicationUserService.GetAllActiveExecutivesDetails());
        }
        /// <summary>
        /// Get Executive Detail By Executive Id
        /// </summary>
        /// <param name="ExecutiveId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetExecutiveDetailById(int ExecutiveId)
        {
            return GetResult(await _applicationUserService.GetExecutiveDetailById(ExecutiveId));
        }
        #endregion
    }
}

using Sporta.Data.Database.Sporta;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporta.WebApp.Services.Interface
{
    /// <summary>
    /// Application User Service Interface
    /// </summary>
    public interface IApplicationUserService
    {
        /// <summary>
        /// Get Application Executives List
        /// </summary>
        /// <returns>Collection of Application Users</returns>
        public Task<ServiceResult<IEnumerable<ApplicationUserModel>>> GetExecutives();

        /// <summary>
        /// Get Application User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Application User based on ID</returns>
        public Task<ServiceResult<ApplicationUserModel>> GetApplicationUser(int Id, int roleId);

        /// <summary>
        /// Signup Application User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public Task<ServiceResult<LoginViewModel>> SignupUser(ApplicationUserRequestModel user, int createdBy);

        /// <summary>
        /// Add Application User
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public Task<ServiceResult<ApplicationUser>> AddExecutive(ApplicationUserRequestModel applicationUser, int createdBy);

        /// <summary>
        /// Update Application User 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="modifiedby"></param>
        /// <returns></returns>
        public Task<ServiceResult<ApplicationUser>> UpdateUser(ApplicationUserRequestModel user, int modifiedby);

        /// <summary>
        /// Remove Application User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Task<ServiceResult<bool>> RemoveUser(int id, int roleId, int modifiedBy);

        /// <summary>
        /// Delete Application User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ServiceResult<bool>> DeleteUser(int id);

        /// <summary>
        /// Signin
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        public Task<ServiceResult<LoginResponseModel>> Signin(LoginViewModel loginViewModel);

        /// <summary>
        /// Get All Drives
        /// </summary>
        /// <returns></returns>
        public Task<ServiceResult<IEnumerable<DriveDropDown>>> GetAllDrives();

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<ServiceResult<bool>> ChangePassword(string newPassword, string oldPassword, int Id);

        /// <summary>
        /// Get Active Executives Details
        /// </summary>
        /// <returns></returns>
        public Task<ServiceResult<IEnumerable<ActiveExecutiveResponseModel>>> GetAllActiveExecutivesDetails();
        /// Get Executive Detail By Id
        /// </summary>
        /// <param name="ExecutiveId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<ExecutiveDriveResponseModel>>> GetExecutiveDetailById(int ExecutiveId);

    }
}

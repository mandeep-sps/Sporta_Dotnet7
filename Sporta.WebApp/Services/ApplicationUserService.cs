using Sporta.Data.Database.Sporta;
using Sporta.Data.Models;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Repository;
using Sporta.WebApp.Services.Interface;
using Sporta.WebApp.StoreProcedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sporta.WebApp.Services
{
    /// <summary>
    /// Application User Service
    /// </summary>
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ISportaRepository<ApplicationUser> _applictionUserRepo;
        private readonly ISportaRepository<Candidate> _candidateRepo;
        private readonly Sporta_DbContext _context;
        private readonly ISportaRepository<Drive> _driveRepo;
        private readonly IResultService _resultService;
        private readonly IBaseStoredProc _resultProc;

        public ApplicationUserService(ISportaRepository<ApplicationUser> applictionUserRepo,
            ISportaRepository<Candidate> candidateRepo, ISportaRepository<Drive> driveRepo,
            IResultService resultService, Sporta_DbContext context, IBaseStoredProc resultProc)
        {
            _applictionUserRepo = applictionUserRepo;
            _candidateRepo = candidateRepo;
            _driveRepo = driveRepo;
            _context = context;
            _resultService = resultService;
            _resultProc = resultProc;
        }


        ///// <summary>
        ///// Get Executive details
        ///// </summary>
        ///// <param name="ExecutiveId"></param>
        ///// <returns></returns>
        public async Task<ServiceResult<IEnumerable<ActiveExecutiveResponseModel>>> GetAllActiveExecutivesDetails()
        {
            try
            {
               
                var list = await _resultProc.ExecuteStoredProcActiveExecutiveDetailsAsync<GetActiveExecutivesDetails_Result>("Eecutive_Details");
                if (list != null)
                {
                    var executiveDetails = list.Select(x => new ActiveExecutiveResponseModel
                    {
                        Name = x.Name,
                        Email = x.Username,
                        DriveName = x.DriveName,
                    });
                    return new ServiceResult<IEnumerable<ActiveExecutiveResponseModel>>(executiveDetails, "Executives List!");
                }
                else
                {
                    return new ServiceResult<IEnumerable<ActiveExecutiveResponseModel>>(null, "No Active Executive Found");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<ActiveExecutiveResponseModel>>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Executives List
        /// </summary>
        /// <returns> List of Application Users</returns>
        public async Task<ServiceResult<IEnumerable<ApplicationUserModel>>> GetExecutives()
        {
            try
            {
                var list = await _applictionUserRepo.GetAllAsync(x => x.ApplicationRoleId == (int)UserRole.Executive, a => a.ApplicationRole,
                     a => a.ApplicationRole);
                var applicationUsers = list.Select(x => new ApplicationUserModel
                {
                    Id = x.Id,
                    ApplicationRole = x.ApplicationRole.RoleName,
                    ApplicationRoleId = x.ApplicationRoleId,
                    Name = x.Name,
                    Username = x.Username,
                });

                return new ServiceResult<IEnumerable<ApplicationUserModel>>(applicationUsers, "Users List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<ApplicationUserModel>>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Executive detail by Executive Id
        /// </summary>
        /// <param name="ExecutiveId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<ExecutiveDriveResponseModel>>> GetExecutiveDetailById(int ExecutiveId)
        {
            try
            {
                var list = await _resultProc.ExecuteStoredProcCollectionAsync<GetExecutiveDetailByExecutiveId_Result>("GetExecutiveDetailByExecutiveId", ExecutiveId);
                
                if(list.Any())
                {
                    var executiveList = list.Select(x => new ExecutiveDriveResponseModel
                    {
                        ExecutiveId = x.ApplicationUserId,
                        ExecutiveName = x.Name,
                        DriveId = x.DriveId,
                        DriveName = x.DriveName,
                        EnrolledCount = x.Enrolled,
                        ScheduledTime = x.Scheduled,
                        Token = x.Token

                    });

                    return new ServiceResult<IEnumerable<ExecutiveDriveResponseModel>>(executiveList, "Executive List!");
                }
                else
                {
                    return new ServiceResult<IEnumerable<ExecutiveDriveResponseModel>>(null, "No Drive is assigned yet!");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<ExecutiveDriveResponseModel>>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Application User based on Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Application User Based on User ID</returns>
        public async Task<ServiceResult<ApplicationUserModel>> GetApplicationUser(int userId, int roleId)
        {
            try
            {
                var user = await _applictionUserRepo.GetAsync(x => x.Id == userId,
                    a => a.ApplicationRole, b => b.Candidate);

                ApplicationUserModel applicationUser = new();
                applicationUser.CandidateRequestModel = new();

                applicationUser.Id = user.Id;
                applicationUser.ApplicationRole = user.ApplicationRole.RoleName;
                applicationUser.ApplicationRoleId = user.ApplicationRoleId;
                applicationUser.Name = user.Name;
                applicationUser.Username = user.Username;

                if (roleId == (int)UserRole.Candidate)
                {
                    applicationUser.CandidateRequestModel.Branch = user.Candidate.Branch;
                    applicationUser.CandidateRequestModel.RollNo = user.Candidate.RollNo;
                    applicationUser.CandidateRequestModel.ContactNumber = user.Candidate.ContactNumber;
                    applicationUser.CandidateRequestModel.DriveId = user.Candidate.DriveId;
                }
                var message = roleId == 3 ? "Candidate Details!" : "Executive Details!";

                return new ServiceResult<ApplicationUserModel>(applicationUser, message);
            }
            catch (Exception ex)
            {
                return new ServiceResult<ApplicationUserModel>(ex, ex.Message);
            }


        }


        /// <summary>
        /// Add User / Add Candidate
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LoginViewModel>> SignupUser(ApplicationUserRequestModel applicationUser, int createdBy)
        {
            try
            {
                var drive = await _driveRepo.GetAsync(x => x.Token == applicationUser.CandidateRequestModel.Token);
                if (drive != null)
                {
                    var contextModel = new ApplicationUser
                    {
                        Name = applicationUser.Name,
                        Username = applicationUser.EmailId,
                        UserPassword = applicationUser.CandidateRequestModel.Token.ToString(),
                        ApplicationRoleId = applicationUser.ApplicationRoleId,
                        IsActive = true,
                        IsDeleted = false
                    };

                    // Checks email for duplicate
                    if (await _applictionUserRepo.IsExistsAsync(au => au.Username.Equals(applicationUser.EmailId) && au.Candidate.DriveId == drive.Id))
                        return new ServiceResult<LoginViewModel>(null, $"Email Already Exist with {drive.DriveName}", true);

                    if (await _applictionUserRepo.IsExistsAsync(au => au.Candidate.ContactNumber.Equals(applicationUser.CandidateRequestModel.ContactNumber) && au.Candidate.DriveId == drive.Id))
                        return new ServiceResult<LoginViewModel>(null, $"Contact No. Already Exist with {drive.DriveName}.", true);

                    if (!string.IsNullOrEmpty(applicationUser.CandidateRequestModel.RollNo))
                    {
                        if (await _candidateRepo.IsExistsAsync(au => au.RollNo.Equals(applicationUser.CandidateRequestModel.RollNo) && au.DriveId == drive.Id))
                            return new ServiceResult<LoginViewModel>(null, $"Roll No. Already exist with {drive.DriveName}.", true);
                    }

                    await _applictionUserRepo.AddAsync(contextModel, createdBy);
                    await _applictionUserRepo.SaveChangesAsync();

                    var candidateModel = new Candidate
                    {
                        Id = contextModel.Id,
                        ContactNumber = applicationUser.CandidateRequestModel.ContactNumber,
                        IsActive = true,
                        IsDeleted = false,
                        CandidateStatusId = 1,
                        DriveId = drive.Id,
                        RollNo = applicationUser.CandidateRequestModel.RollNo,
                        Branch = applicationUser.CandidateRequestModel.Branch
                    };

                    await _candidateRepo.AddAsync(candidateModel, createdBy);

                    drive.Enrolled += 1;

                    _driveRepo.Update(drive, createdBy);

                    await _candidateRepo.SaveChangesAsync();

                    await _resultService.AddCandidateResult(contextModel.Id);

                    return new ServiceResult<LoginViewModel>(new LoginViewModel { EmailId = contextModel.Username, UserPassword = contextModel.UserPassword, IsFromSignup = true }, "User added!");
                }
                else
                    return new ServiceResult<LoginViewModel>(null, "Token number expried!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<LoginViewModel>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Add User Executive
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ApplicationUser>> AddExecutive(ApplicationUserRequestModel applicationUser, int createdBy)
        {
            try
            {
                var contextModel = new ApplicationUser
                {
                    Name = applicationUser.Name,
                    Username = applicationUser.EmailId,
                    UserPassword = applicationUser.UserPassword,
                    ApplicationRoleId = (int)UserRole.Executive,
                    IsActive = true,
                    IsDeleted = false
                };

                // Checks email for duplicate
                if (await _applictionUserRepo.IsExistsAsync(au => au.Username.Equals(applicationUser.EmailId)))
                    return new ServiceResult<ApplicationUser>(contextModel, "Email Already Exist!", true);

                await _applictionUserRepo.AddAsync(contextModel, createdBy);
                await _applictionUserRepo.SaveChangesAsync();


                return new ServiceResult<ApplicationUser>(contextModel, "Executive added!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<ApplicationUser>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Update User 
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ApplicationUser>> UpdateUser(ApplicationUserRequestModel applicationUser, int modifiedBy)
        {
            try
            {
                var contextModel = await _applictionUserRepo.GetAsync(x => x.Id == applicationUser.Id);
                if (contextModel != null)
                {
                    contextModel.Name = applicationUser.Name;

                    _applictionUserRepo.Update(contextModel, modifiedBy);
                    await _applictionUserRepo.SaveChangesAsync();
                }

                return new ServiceResult<ApplicationUser>(contextModel, "Executive updated!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<ApplicationUser>(ex, ex.Message);
            }

        }


        /// <summary>
        /// Get All Drives
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<DriveDropDown>>> GetAllDrives()
        {
            try
            {
                var list = await _driveRepo.GetAllAsync();
                var dropDown = list.Select(x => new DriveDropDown
                {
                    Id = x.Id,
                    DriveName = x.DriveName

                });

                return new ServiceResult<IEnumerable<DriveDropDown>>(dropDown, "Dropdown List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<DriveDropDown>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Remove User
        /// </summary>
        /// <param name="modifiedBy"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> RemoveUser(int userId, int roleId, int modifiedBy)
        {
            try
            {
                if (roleId == (int)UserRole.Candidate)
                {
                    await _candidateRepo.Remove(userId, modifiedBy);
                    await _applictionUserRepo.SaveChangesAsync();
                    await _applictionUserRepo.Remove(userId, modifiedBy);
                    return new ServiceResult<bool>(await _applictionUserRepo.SaveChangesAsync(), "Candidate deleted Successfully!");
                }
                else
                {
                    if (_context.DriveAssignees.Any(x => x.ApplicationUserId == userId && !x.IsDeleted))
                    {
                        return new ServiceResult<bool>(false, "Executive already assigned in a drive.", true);
                    }
                    await _applictionUserRepo.Remove(userId, modifiedBy);
                    return new ServiceResult<bool>(await _applictionUserRepo.SaveChangesAsync(), "Executive deleted Successfully!");
                }

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }

        }


        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteUser(int id)
        {
            try
            {
                var applicationUser = await _applictionUserRepo.GetAsync(x => x.Id == id);
                if (applicationUser != null)
                    _applictionUserRepo.Delete(applicationUser);

                return new ServiceResult<bool>(true, "User Deleted Successfully!");
            }
            catch (Exception ex)
            {

                return new ServiceResult<bool>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Signin
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LoginResponseModel>> Signin(LoginViewModel login)
        {
            try
            {
                var applicationUsersList = await _applictionUserRepo.GetAllAsync(x => x.Username.ToLower().Equals(login.EmailId.ToLower()));


                if (applicationUsersList.Any())
                {
                    var applicationUser = applicationUsersList.FirstOrDefault(x => x.UserPassword.Equals(login.UserPassword));
                    if (applicationUser != null)
                    {
                        if (!applicationUser.IsActive && applicationUser.ApplicationRoleId != (int)UserRole.Candidate)
                            return new ServiceResult<LoginResponseModel>(null, "Your Account is blocked, please contact admin.", true);


                        if (applicationUser.ApplicationRoleId == (int)UserRole.Candidate)
                        {
                            var candidate = await _candidateRepo.GetAsync(x => x.Id == applicationUser.Id);
                            var drive = await _driveRepo.GetAsync(x => x.Token.ToString() == login.UserPassword);

                            if (drive == null)
                            {
                                return new ServiceResult<LoginResponseModel>(null, "Drive has been expired!", true);
                            }
                            else if (!drive.IsActive)
                            {
                                var message = $"Drive: {drive.DriveName} is Schedule for {drive.Scheduled:dd-MM-yyyy hh:mm tt},<br> Please login on scheduled time.";
                                return new ServiceResult<LoginResponseModel>(new LoginResponseModel { ResponseType = "danger" },
                                   login.IsFromSignup ? $"Signup process completed. <br> {message}" : message, true);
                            }
                            else if (candidate.CandidateStatusId == 3)
                            {
                                return new ServiceResult<LoginResponseModel>(new LoginResponseModel { ResponseType = "info" },
                                    "You had submitted test.<br>Result will be given by HR, Please Contact HR for the update.", true);
                            }
                        }

                        LoginResponseModel loginResponse = new();

                        loginResponse.RoleId = applicationUser.ApplicationRoleId;
                        loginResponse.UserId = applicationUser.Id;
                        loginResponse.UserName = applicationUser.Username;
                        loginResponse.Name = $"{applicationUser.Name}";

                        return new ServiceResult<LoginResponseModel>(loginResponse, "User Details");

                    }
                    else
                        return new ServiceResult<LoginResponseModel>(null, "Invalid Credentials!");
                }
                else
                    return new ServiceResult<LoginResponseModel>(null, "User not found, please contact admin.");
            }
            catch (Exception ex)
            {
                return new ServiceResult<LoginResponseModel>(ex, ex.Message);
            }

        }


        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> ChangePassword(string newPassword, string oldPassword, int Id)
        {
            try
            {
                var user = await _applictionUserRepo.GetAsync(x => x.Id == Id);

                if (user.UserPassword == oldPassword)
                {
                    user.UserPassword = newPassword;

                    _applictionUserRepo.Update(user, Id);
                    return new ServiceResult<bool>(await _applictionUserRepo.SaveChangesAsync(), "Password Updated Successfully!");
                }
                else
                {
                    return new ServiceResult<bool>(false, "Old Password Is Incorrect!");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }

        }
    }
}

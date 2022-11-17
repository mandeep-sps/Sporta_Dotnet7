using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Sporta.Data.Database.Sporta;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Repository;
using Sporta.WebApp.Services.Interface;
using Sporta.WebApp.StoreProcedures;
using Sporta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sporta.WebApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Sporta.WebApp.Services
{
    public class DriveService : IDriveService
    {
        private readonly ISportaRepository<Drive> _driveRepo;
        private readonly ISportaRepository<DriveAssignee> _driveAssigneeRepo;
        private readonly ISportaRepository<ApplicationUser> _userRepo;
        private readonly ISportaRepository<Result> _resultRepo;
        private readonly ISportaRepositoryV2 _sportaRepositoryV2;
        private readonly ISportaRepository<Candidate> _candidateRepo;
        private readonly Sporta_DbContext _context;
        private readonly IBaseStoredProc _resultProc;
        private readonly IHubContext<DashboardHub> _hubContext;

        public DriveService(
            ISportaRepository<Drive> driveRepo,
            ISportaRepository<DriveAssignee> driveAssigneeRepo,
            ISportaRepository<ApplicationUser> userRepo,
            ISportaRepository<Result> resultRepo,
            ISportaRepository<Candidate> candidateRepo,
            ISportaRepositoryV2 sportaRepositoryV2,
            Sporta_DbContext context,
            IBaseStoredProc resultProc,
            IHubContext<DashboardHub> hubContext)
        {
            _driveRepo = driveRepo;
            _driveAssigneeRepo = driveAssigneeRepo;
            _userRepo = userRepo;
            _resultRepo = resultRepo;
            _context = context;
            _sportaRepositoryV2 = sportaRepositoryV2;
            _candidateRepo = candidateRepo;
            _resultProc = resultProc;
            _hubContext = hubContext;
        }

        #region Drive CRUD Actions

        /// <summary>
        /// Get Drives
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<DriveResponseModel>>> GetDrives(int userId, bool isLive, bool isUpcomming)
        {
            try
            {
                var userDrives = (await _driveAssigneeRepo.GetAllAsync(x => x.ApplicationUserId == userId)).Select(x => x.DriveId);
                var executiveDrives = await _driveAssigneeRepo.GetAllAsync(x => !userDrives.Any() || userDrives.Contains(x.DriveId), AU => AU.ApplicationUser);
                var dt = DateTime.Now;
                var drives = new List<Drive>();
                if (isLive)
                {
                    drives = await _context.Drives.
                    Where(x => executiveDrives.Select(y => y.DriveId).Contains(x.Id) && !x.IsDeleted && x.IsActive && x.Scheduled >= dt)
                    .Include(DC => DC.DriveCategories)
                    .ThenInclude(C => C.Category)
                    .AsNoTracking().ToListAsync();
                }
                else if (isUpcomming)
                {
                    drives = await _context.Drives.
                       Where(x => executiveDrives.Select(y => y.DriveId).Contains(x.Id) && !x.IsDeleted && !x.IsActive && x.Scheduled > dt)
                       .Include(DC => DC.DriveCategories)
                       .ThenInclude(C => C.Category)
                       .AsNoTracking().ToListAsync();

                }
                else
                {
                    drives = await _context.Drives.
                                           Where(x => executiveDrives.Select(y => y.DriveId).Contains(x.Id) && !x.IsDeleted && x.Scheduled >= dt.AddMonths(-2))
                                           .Include(DC => DC.DriveCategories)
                                           .ThenInclude(C => C.Category)
                                           .AsNoTracking().ToListAsync();

                }




                //var drives = await _context.Drives.
                //    Where(x => (executiveDrives.Select(y => y.DriveId).Contains(x.Id)) && !x.IsDeleted &&
                //    //isLive ? x.IsActive && x.Scheduled >= dt : isUpcomming ? !x.IsActive && x.Scheduled > dt : x.Scheduled >= dt.AddMonths(-2) && !x.IsDeleted)
                //    .Include(DC => DC.DriveCategories)
                //    .ThenInclude(C => C.Category)
                //    .AsNoTracking().ToListAsync();

                var data = new ServiceResult<IEnumerable<DriveResponseModel>>(drives.Select(x => new DriveResponseModel
                {
                    Id = x.Id,
                    DriveName = x.DriveName,
                    Scheduled = x.Scheduled,
                    DriveCategory = x.DriveCategories.Select(x => new DriveCategoryModel
                    {
                        CategoryId = x.CategoryId,
                        CategoryTime = Convert.ToInt32(x.AllotedTime),
                        CategoryQuestion = x.TotalQuestion
                    }),
                    Categories = string.Join(", ", x.DriveCategories.Select(x => x.Category.CategoryName)),
                    DriveAssignee = executiveDrives.Where(y => y.DriveId == x.Id).Select(x => x.ApplicationUserId),
                    Assignees = string.Join(", ", executiveDrives.Where(y => y.DriveId == x.Id).Select(x => x.ApplicationUser.Name)),
                    Enrolled = x.Enrolled,
                    Token = x.Token,
                    AllotedTime = x.DriveCategories.Sum(x => Convert.ToInt32(x.AllotedTime)),
                    TotalQuestion = x.DriveCategories.Sum(x => x.TotalQuestion),
                    Status = x.IsActive
                }), "Drvies List");
                return data;
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<DriveResponseModel>>(ex, ex.Message);
            }

        }



        /// <summary>
        /// Get Drive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<DriveResponseModel>> GetDrive(int id)
        {
            DriveResponseModel obj = new();
            var drives = await _context.Drives.Where(x => !x.IsDeleted && x.Id == id)
                .Include(DC => DC.DriveCategories.Where(x => x.IsDeleted == false))
                .ThenInclude(C => C.Category)
                .Include(DA => DA.DriveAssignees.Where(x => !x.IsDeleted))
                .ThenInclude(U => U.ApplicationUser).AsNoTracking()
                .FirstOrDefaultAsync();
            if (drives != null)
            {
                return new ServiceResult<DriveResponseModel>(new DriveResponseModel
                {
                    Id = drives.Id,
                    DriveName = drives.DriveName,
                    Scheduled = drives.Scheduled,
                    DriveCategory = drives.DriveCategories.Select(x => new DriveCategoryModel
                    {
                        CategoryId = x.CategoryId,
                        Category = x.Category.CategoryName,
                        CategoryTime = Convert.ToInt32(x.AllotedTime),
                        CategoryQuestion = x.TotalQuestion,
                        SelectedQuestions = x.QuestionIds,
                    }),
                    Categories = string.Join(", ", drives.DriveCategories.Select(x => x.Category.CategoryName)),
                    DriveAssignee = drives.DriveAssignees.Select(x => x.ApplicationUserId),
                    Assignees = string.Join(", ", drives.DriveAssignees.Select(x => x.ApplicationUser.Name)),
                    Enrolled = drives.Enrolled,
                    AllotedTime = drives.DriveCategories.Sum(x => Convert.ToInt32(x.AllotedTime)),
                    TotalQuestion = drives.DriveCategories.Sum(x => x.TotalQuestion),
                    Status = drives.IsActive
                }, "Drive Data");
            }
            return new ServiceResult<DriveResponseModel>(obj, "no record found");
        }


        /// <summary>
        /// Get Drive By Candidate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<CandidateDriveReponseModel>> GetDriveByCandidate(int id)
        {
            try
            {
                var user = await _userRepo.GetAsync(x => x.Id == id, C => C.Candidate);
                var driveId = user.Candidate.DriveId;
                var result = await _resultRepo.GetAsync(x => x.CandidateId == user.Candidate.Id, RD => RD.ResultDetails);

                var drive = await _context.Drives.Where(x => !x.IsDeleted && x.Id == driveId)
                 .Include(DC => DC.DriveCategories)
                 .ThenInclude(C => C.Category)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();

                var reponse = new CandidateDriveReponseModel
                {
                    DriveId = drive.Id,
                    DriveName = drive.DriveName,
                    Scheduled = drive.Scheduled,
                    Categories = drive.DriveCategories.Select(x => new DriveCategoryModel
                    {
                        CategoryId = x.CategoryId,
                        Category = x.Category.CategoryName,
                        CategoryTime = Convert.ToInt32(x.AllotedTime),
                        CategoryQuestion = x.TotalQuestion,
                        SelectedQuestions =x.QuestionIds,
                        IsAttempted = result != null ? (result.ResultDetails.Any(y => y.CategoryId == x.CategoryId && y.ResultId == result.Id) ?
                        result.ResultDetails.Where(y => y.CategoryId == x.CategoryId && y.ResultId == result.Id).FirstOrDefault().IsAttempted : false) : false
                    }),
                };

                return new ServiceResult<CandidateDriveReponseModel>(reponse, "Candidate's drive data");
            }
            catch (Exception ex)
            {
                return new ServiceResult<CandidateDriveReponseModel>(ex, ex.Message);
            }

        }


        /// <summary>
        /// Create Drive
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<DriveResponseModel>> CreateDrive(DriveRequestModel request, int createdBy)
        {
            try
            {
                var driveContext = new Drive
                {
                    DriveName = request.DriveName,
                    Scheduled = request.Scheduled,
                    Enrolled = 0, //at the time of drive creation, no candidate is enrolled.
                    Token = await GetRandomToken(),
                    IsActive = false
                };

                await _driveRepo.AddAsync(driveContext, createdBy);
                await _driveRepo.SaveChangesAsync();


                await AddUpdateDriveCategories(request.DriveCategories, driveContext.Id, createdBy, IsNewRecord: true);
                if (request.DriveAssignee == null)
                    request.DriveAssignee = new List<int> { createdBy };
                await AddUpdateDriveAssignees(request.DriveAssignee, driveContext.Id, createdBy, IsNewRecord: true);
                await _hubContext.Clients.All.SendAsync("NewDriveCreated", request);
                return new ServiceResult<DriveResponseModel>((await GetDrive(driveContext.Id)).Data,
                    "Drive created successfully!<br> Send the drive token to respective candidates to initialize the signup process.");
            }
            catch (Exception ex)
            {
                return new ServiceResult<DriveResponseModel>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Update Drive
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<DriveResponseModel>> UpdateDrive(DriveRequestModel request, int modifiedBy)
        {
            try
            {
                var driveContext = await _driveRepo.GetAsync(x => x.Id == request.Id);

                driveContext.DriveName = request.DriveName;
                driveContext.Scheduled = request.Scheduled;

                _driveRepo.Update(driveContext, modifiedBy);
                await _driveRepo.SaveChangesAsync();

                await AddUpdateDriveCategories(request.DriveCategories, driveContext.Id, modifiedBy, IsNewRecord: false);
                if (request.DriveAssignee != null)
                    await AddUpdateDriveAssignees(request.DriveAssignee, driveContext.Id, modifiedBy, IsNewRecord: false);

                return new ServiceResult<DriveResponseModel>((await GetDrive(driveContext.Id)).Data, "Drive Updated Successfully!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<DriveResponseModel>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Remove Drive
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> RemoveDrive(int id, int modifiedBy)
        {
            try
            {
                await RemoveCandidates(id, modifiedBy);
                await RemoveAssignee(id, modifiedBy);
                await _driveRepo.Remove(id, modifiedBy);

                return new ServiceResult<bool>(await _driveRepo.SaveChangesAsync(), "Drive Moved to Archive.");
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Delete Drive Permanently
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteDrivePermanently(int id)
        {
            try
            {
                var drive = await _sportaRepositoryV2.GetAllAsync<Drive>(x => x.Id == id);
                if (drive.Any())
                {
                    var driveAssignees = await _sportaRepositoryV2.GetAllAsync<DriveAssignee>(x => x.DriveId == id);
                    await _context.BulkDeleteAsync(driveAssignees.ToList());

                    var driveCategory = await _sportaRepositoryV2.GetAllAsync<DriveCategory>(x => x.DriveId == id);
                    await _context.BulkDeleteAsync(driveCategory.ToList());

                    var candidates = await _sportaRepositoryV2.GetAllAsync<Candidate>(x => x.DriveId == id, au => au.IdNavigation);

                    foreach (var candidate in candidates)
                    {
                        var results = await _sportaRepositoryV2.GetAllAsync<Result>(x => x.CandidateId == candidate.Id);
                        if (results.Any())
                        {
                            var resultDetail = await _sportaRepositoryV2.GetAllAsync<ResultDetail>(r => r.ResultId == results.FirstOrDefault().Id);
                            await _context.BulkDeleteAsync(resultDetail.ToList());

                            await _context.BulkDeleteAsync(results.ToList());
                        }
                        _sportaRepositoryV2.Delete(candidate);
                        _sportaRepositoryV2.Delete(candidate.IdNavigation);
                    }

                    _sportaRepositoryV2.Delete(drive.FirstOrDefault());

                    return new ServiceResult<bool>(await _driveRepo.SaveChangesAsync(), "Drive Removed Successfully!");
                }
                return new ServiceResult<bool>(false, "Drive not available", true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Enable / Disable Drive
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> EnableDisableDrive(int id, bool status, int modifiedBy)
        {
            try
            {
                var drive = await _driveRepo.GetAsync(x => x.Id == id);

                if (drive != null)
                {
                    drive.IsActive = status;

                    _driveRepo.Update(drive, modifiedBy);

                    await EnableDisableCandidates(id, drive.IsActive, modifiedBy);

                    return new ServiceResult<bool>(await _driveRepo.SaveChangesAsync(), "Drive Status Updated!");
                }

                return new ServiceResult<bool>(false, "No record found!", true);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Get Filtered Archived Drives
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<DriveResponseModel>>> GetFilteredArchivedDrives(DriveFilterRequestModel filters, int userId)
        {
            try
            {
                Expression<Func<Drive, bool>> driveFilterExpression = dfe =>
                dfe.IsDeleted && (string.IsNullOrEmpty(filters.DriveName) ||
                dfe.DriveName.ToLower().Trim().Contains(filters.DriveName.ToLower().Trim())) &&
                (filters.FromDate == null || dfe.Scheduled >= filters.FromDate) &&
                (filters.ToDate == null || dfe.Scheduled <= filters.ToDate);

                List<Drive> drives = new();

                var executiveDrives = await _sportaRepositoryV2.GetAllAsync<DriveAssignee>(x => (x.ApplicationUserId == userId) || userId == 0, AU => AU.ApplicationUser);

                if (string.IsNullOrEmpty(filters.DriveName) && filters.FromDate == null && filters.ToDate == null)
                {
                    drives = await _context.Drives.Where(x => executiveDrives.Select(y => y.DriveId).Contains(x.Id))
                    .Where(driveFilterExpression)
                    .Include(DC => DC.DriveCategories)
                    .ThenInclude(C => C.Category).Take(20)
                    .AsNoTracking().ToListAsync();
                }
                else
                {
                    drives = await _context.Drives.Where(x => executiveDrives.Select(y => y.DriveId).Contains(x.Id))
                   .Where(driveFilterExpression)
                   .Include(DC => DC.DriveCategories)
                   .ThenInclude(C => C.Category)
                   .AsNoTracking().ToListAsync();
                }

                return new ServiceResult<IEnumerable<DriveResponseModel>>(drives.Select(x => new DriveResponseModel
                {
                    Id = x.Id,
                    DriveName = x.DriveName,
                    Scheduled = x.Scheduled,
                    DriveCategory = x.DriveCategories.Select(x => new DriveCategoryModel
                    {
                        CategoryId = x.CategoryId,
                        CategoryTime = Convert.ToInt32(x.AllotedTime),
                        CategoryQuestion = x.TotalQuestion
                    }),
                    Categories = string.Join(", ", x.DriveCategories.Select(x => x.Category.CategoryName)),
                    DriveAssignee = executiveDrives.Where(y => y.DriveId == x.Id).Select(x => x.ApplicationUserId),
                    Assignees = string.Join(", ", executiveDrives.Where(y => y.DriveId == x.Id).Select(x => x.ApplicationUser.Name)),
                    Enrolled = x.Enrolled,
                    Token = x.Token,
                    AllotedTime = x.DriveCategories.Sum(x => Convert.ToInt32(x.AllotedTime)),
                    TotalQuestion = x.DriveCategories.Sum(x => x.TotalQuestion),
                    Status = x.IsActive
                }), "Drvies List");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<DriveResponseModel>>(ex, ex.Message);
            }

        }

        #endregion


        #region Extensions / Helpers

        /// <summary>
        /// Get Random Token
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetRandomToken()
        {
            var token = CommonMethods.GenerateRandomToken();
            if (await _driveRepo.IsExistsAsync(x => x.Token == token))
                await GetRandomToken();

            return token;
        }


        /// <summary>
        /// Enable Disable Candidates
        /// </summary>
        /// <param name="driveId"></param>
        /// <param name="status"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        private async Task<bool> EnableDisableCandidates(int driveId, bool status, int modifiedBy)
        {
            var users = await _userRepo.GetAllAsync(x => x.ApplicationRoleId == (int)UserRole.Candidate && x.Candidate.DriveId == driveId, i => i.Candidate);

            users.ToList().ForEach(_ =>
            {
                _.IsActive = status;
                _.Candidate.IsActive = status;
                _.Candidate.ModifiedOn = DateTime.Now;
                _.Candidate.ModifiedBy = modifiedBy;
                _.ModifiedBy = modifiedBy;
                _.ModifiedOn = DateTime.Now;
            });

            _context.BulkUpdate(users.ToList());

            return true;
        }


        /// <summary>
        /// Remove Candidates
        /// </summary>
        /// <param name="driveId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        private async Task<bool> RemoveCandidates(int driveId, int modifiedBy)
        {
            var users = await _userRepo.GetAllAsync(x => x.ApplicationRoleId == (int)UserRole.Candidate && x.Candidate.DriveId == driveId, i => i.Candidate);

            List<ApplicationUser> applicationUsers = new();

            users.ToList().ForEach(_ =>
            {
                _.IsDeleted = true;
                _.Candidate.IsDeleted = true;
                _.Candidate.ModifiedOn = DateTime.Now;
                _.Candidate.ModifiedBy = modifiedBy;
                _.ModifiedBy = modifiedBy;
                _.ModifiedOn = DateTime.Now;

                _context.Update(_.Candidate);
            });

            //Remove Results
            var results = await _resultRepo.GetAllAsync(x => users.Select(x => x.Candidate.Id).Contains(x.CandidateId), RD => RD.ResultDetails);
            results.ToList().ForEach(_ =>
            {
                _.IsDeleted = true;
                _.ModifiedBy = modifiedBy;
                _.ModifiedOn = DateTime.Now;
                _.ResultDetails.ToList().ForEach(_ =>
                {
                    _.IsDeleted = true;
                    _.ModifiedBy = modifiedBy;
                    _.ModifiedOn = DateTime.Now;
                });

                _context.UpdateRange(_.ResultDetails);
            });

            _context.BulkUpdate(users.ToList());
            _context.BulkUpdate(results.ToList());

            return true;
        }


        /// <summary>
        /// Remove Assignee
        /// </summary>
        /// <param name="driveId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        private async Task<bool> RemoveAssignee(int driveId, int modifiedBy)
        {
            var driveAssignies = await _sportaRepositoryV2.GetAllAsync<DriveAssignee>(x => !x.IsDeleted && x.DriveId == driveId);

            var driveCategories = await _sportaRepositoryV2.GetAllAsync<DriveCategory>(x => !x.IsDeleted && x.DriveId == driveId);

            driveAssignies.ToList().ForEach(x =>
            {
                x.IsDeleted = true;
                x.ModifiedBy = modifiedBy;
                x.ModifiedOn = DateTime.Now;
            });

            driveCategories.ToList().ForEach(x =>
            {
                x.IsDeleted = true;
                x.ModifiedBy = modifiedBy;
                x.ModifiedOn = DateTime.Now;
            });

            _context.BulkUpdate(driveAssignies.ToList());
            _context.BulkUpdate(driveCategories.ToList());

            return await _context.SaveChangesAsync() > 0;
        }


        /// <summary>
        /// Add / Update Drive Categories
        /// </summary>
        /// <param name="driveCategories"></param>
        /// <param name="driveId"></param>
        /// <param name="createdBy"></param>
        /// <param name="IsNewRecord"></param>
        /// <returns></returns>
        private async Task<bool> AddUpdateDriveCategories(IEnumerable<DriveCategoryModel> driveCategories, int driveId, int createdBy, bool IsNewRecord)
        {
            var driveCategoriesContext = new List<DriveCategory>();
            foreach (var driveCategory in driveCategories)
            {
                var dcContext = new DriveCategory
                {
                    DriveId = driveId,
                    CategoryId = driveCategory.CategoryId,
                    AllotedTime = driveCategory.CategoryTime.ToString(),
                    TotalQuestion = driveCategory.CategoryQuestion,
                    QuestionIds = driveCategory.SelectedQuestions,
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = createdBy,
                    ModifiedOn = DateTime.Now,
                    IsDeleted = false
                };

                driveCategoriesContext.Add(dcContext);
            }

            //Remove old records while updating the drive
            if (!IsNewRecord)
                _context.DriveCategories.RemoveRange(_context.DriveCategories.Where(x => x.DriveId == driveId));

            await _context.DriveCategories.AddRangeAsync(driveCategoriesContext);
            return await _context.SaveChangesAsync() > 0;
        }


        /// <summary>
        /// Add / Update Drive Assignees
        /// </summary>
        /// <param name="driveAssignees"></param>
        /// <param name="driveId"></param>
        /// <param name="createdBy"></param>
        /// <param name="IsNewRecord"></param>
        /// <returns></returns>
        private async Task<bool> AddUpdateDriveAssignees(IEnumerable<int> driveAssignees, int driveId, int createdBy, bool IsNewRecord)
        {
            var driveAssigneesContext = new List<DriveAssignee>();
            foreach (var userId in driveAssignees)
            {
                var driveAssignee = new DriveAssignee
                {
                    DriveId = driveId,
                    ApplicationUserId = userId,
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = createdBy,
                    ModifiedOn = DateTime.Now,
                    IsDeleted = false
                };

                driveAssigneesContext.Add(driveAssignee);
            }

            //Remove old records while updating the drive
            if (!IsNewRecord)
                _context.DriveAssignees.RemoveRange(_context.DriveAssignees.Where(x => x.DriveId == driveId));


            await _context.DriveAssignees.AddRangeAsync(driveAssigneesContext);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<ServiceResult<IEnumerable<ApplicationUserModel>>> GetExecutiveDetailsByDriveId(int Id)
        {
            try
            {
                var list = await _resultProc.ExecuteStoredProcDriveExecutivesDetailsAsync<GetExecutivesDetailsByDriveId_Result>("ActiveDriveUserDetails", Id);
                if (list != null)
                {
                    var driveDetails = list.Select(x => new ApplicationUserModel
                    {
                        Id = x.Id,
                        Username = x.Username,
                        Name = x.Name
                    });
                    return new ServiceResult<IEnumerable<ApplicationUserModel>>(driveDetails, "Executives List!");
                }
                else
                {
                    return new ServiceResult<IEnumerable<ApplicationUserModel>>(null, "No Drive is assigned yet!");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<ApplicationUserModel>>(ex, ex.Message);
            }
        }


        #endregion

    }
}

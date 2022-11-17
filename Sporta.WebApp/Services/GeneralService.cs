using Microsoft.EntityFrameworkCore;
using Sporta.Data.Database.Sporta;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Repository;
using Sporta.WebApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sporta.WebApp.Services
{
    /// <summary>
    /// General Service
    /// </summary>
    public class GeneralService : IGeneralService
    {
        private readonly ISportaRepository<Category> _categoryRepo;
        private readonly ISportaRepository<ApplicationUser> _userRepo;
        private readonly ISportaRepository<Drive> _driveRepo;
        private readonly ISportaRepositoryV2 _sportaRepositoryV2;
        private readonly Sporta_DbContext _context;
        public GeneralService(
            ISportaRepository<Category> categoryRepo,
            ISportaRepository<ApplicationUser> userRepo,
            ISportaRepositoryV2 sportaRepositoryV2,
            Sporta_DbContext context,
            ISportaRepository<Drive> driveRepo)
        {
            _categoryRepo = categoryRepo;
            _userRepo = userRepo;
            _driveRepo = driveRepo;
            _sportaRepositoryV2 = sportaRepositoryV2;
            _context = context;
        }

        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<DropdownModel>>> GetList(ListType type, object param = null)
        {
            return type switch
            {
                ListType.Categories => new ServiceResult<IEnumerable<DropdownModel>>
                ((await _categoryRepo.GetAllAsync(x => x.Questions.Count > 0, a => a.Questions)).OrderBy(o => o.CategoryName).Select(x => new DropdownModel
                {
                    Id = x.Id,
                    Name = string.Concat($"{x.CategoryName} ({x.Questions.Count})")
                }), "Category List"),


                ListType.User_Executives => new ServiceResult<IEnumerable<DropdownModel>>
               ((await _userRepo.GetAllAsync(x => x.ApplicationRoleId == (int)UserRole.Executive)).OrderBy(o => o.Name).Select(x => new DropdownModel
               {
                   Id = x.Id,
                   Name = x.Name
               }), "Executive List"),

                ListType.Current_User_Executives => new ServiceResult<IEnumerable<DropdownModel>>
               ((await _userRepo.GetAllAsync(x => x.ApplicationRoleId == (int)UserRole.Executive && x.Id == (int)param)).Select(x => new DropdownModel
               {
                   Id = x.Id,
                   Name = x.Name
               }), "Current Executive List"),


                ListType.Drives => new ServiceResult<IEnumerable<DropdownModel>>
                ((await _driveRepo.GetAllAsync()).OrderByDescending(o => o.CreatedOn).Select(x => new DropdownModel
                {
                    Id = x.Id,
                    Name = x.DriveName
                }), "Drives List"),


                ListType.ArchivedDrives => new ServiceResult<IEnumerable<DropdownModel>>
                ((await _context.Drives.Where(x => x.IsDeleted).AsNoTracking().ToListAsync()).OrderByDescending(o => o.ModifiedOn).Select(x => new DropdownModel
                {
                    Id = x.Id,
                    Name = x.DriveName
                }), "Drives List"),

                ListType.DrivesByExecutives => new ServiceResult<IEnumerable<DropdownModel>>
               ((await _context.DriveAssignees.Include(d => d.Drive)
               .Where(x => x.ApplicationUserId == (int)param && !x.Drive.IsDeleted).AsNoTracking()
               .ToListAsync()).OrderByDescending(o => o.CreatedOn).Select(x => new DropdownModel
               {
                   Id = x.Drive.Id,
                   Name = x.Drive.DriveName
               }), "Drives List"),

                ListType.ArchivedDrivesByExecutives => new ServiceResult<IEnumerable<DropdownModel>>
               ((await _context.DriveAssignees.Include(d => d.Drive)
               .Where(x => x.ApplicationUserId == (int)param && x.Drive.IsDeleted)
               .AsNoTracking().ToListAsync()).OrderByDescending(o => o.ModifiedOn).Select(x => new DropdownModel
               {
                   Id = x.Drive.Id,
                   Name = x.Drive.DriveName
               }), "Drives List"),

                ListType.Branches => new ServiceResult<IEnumerable<DropdownModel>>((await _context.Branches.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync())
                .Select(x => new DropdownModel
                {
                    Id = x.Id,
                    Name = x.BranchName
                }), "Branches List"),

                _ => new ServiceResult<IEnumerable<DropdownModel>>(null, "List type not found!", true),
            };
        }

        /// <summary>
        /// Create Error Log
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResult<Guid>> CreateErrorLog(ErrorRequest errorRequest)
        {
            try
            {
                ErrorLog errorLog = new()
                {
                    Id = Guid.NewGuid(),
                    Information = errorRequest.Information,
                    UserId = errorRequest.UserId,
                    LogTime = DateTime.Now
                };

                await _context.AddAsync(errorLog);
                await _context.SaveChangesAsync();

                return new ServiceResult<Guid>(errorLog.Id, "Exception Logged");
            }
            catch (Exception ex)
            {
                return new ServiceResult<Guid>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Error By Id
        /// </summary>
        /// <param name="errorId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResult<ErrorRequest>> GetErrorById(Guid errorId)
        {
            try
            {
                var errorLog = (await _sportaRepositoryV2.GetAllAsync<ErrorLog>(x => x.Id == errorId)).FirstOrDefault();

                ErrorRequest errorRequest = new();
                if (errorLog != null)
                {
                    errorRequest.Id = errorLog.Id;
                    errorRequest.Information = errorLog.Information;
                    errorRequest.UserId = errorLog.UserId;
                    errorRequest.LogTime = errorLog.LogTime;
                    errorRequest.Username = await GetUsername(errorLog.UserId);

                    return new ServiceResult<ErrorRequest>(errorRequest, "Error Details");
                }

                return new ServiceResult<ErrorRequest>(errorRequest, "Error not found", true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<ErrorRequest>(ex, ex.Message);
            }
        }

        private async Task<string> GetUsername(int id)
        {
            var user = (await _sportaRepositoryV2.GetAllAsync<ApplicationUser>(x => x.Id == id)).FirstOrDefault();
            return user != null ? user.Username : string.Empty;
        }
    }
}

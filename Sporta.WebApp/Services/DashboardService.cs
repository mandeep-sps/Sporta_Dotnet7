using EFCore.BulkExtensions;
using Sporta.Data.Database.Sporta;
using Sporta.Data.Models;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Repository;
using Sporta.WebApp.Services.Interface;
using Sporta.WebApp.Services;
using Sporta.WebApp.StoreProcedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sporta.WebApp.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly Sporta_DbContext _context;
        private readonly ISportaRepository<Category> _categoryRepo;
        private readonly IBaseStoredProc _appProc;

        public DashboardService(
            Sporta_DbContext context,
            ISportaRepository<Category> categoryRepo,
            IBaseStoredProc appProc)
        {
            _context = context;
            _categoryRepo = categoryRepo;
            _appProc = appProc;
        }

        /// <summary>
        /// Get Total Drive Count
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> GetTotalDriveCount(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    var driveCount = await _context.Drives.CountAsync();
                    //return driveCount;
                    return new ServiceResult<int>(driveCount, "Total drive's count");
                }
                else
                {
                    var executiveDrivesCount = await _context.DriveAssignees.AsNoTracking()
                        .Where(c => c.ApplicationUserId == userId).CountAsync();
                    //return executiveDrivesCount;
                    return new ServiceResult<int>(executiveDrivesCount, "Total executive's drive count");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Current Drive Count
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> CurrentLiveDrive(int userId)
        {

            try
            {
                if (userId == 0)
                {
                    var now = DateTime.Now;
                    var driveCount = await _context.Drives.AsNoTracking().Where(c => c.IsActive == true && c.Scheduled >= now).CountAsync();
                    return new ServiceResult<int>(driveCount, "Current live drive's count");
                }
                else
                {
                    var now = DateTime.Now;
                    var driveCount = await _appProc.ExecuteStoredProcCollectionAsync<GetTotalCountByExecutiveID_Result>("GetTotalCountByExecutiveID", userId, now, true, false);
                    int count = driveCount.Select(e => e.DriveCount).First();
                    return new ServiceResult<int>(count, "Current Executive's live drive count");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Upcoming Drive Count
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> UpcomingDrive(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    var now = DateTime.Now;
                    var driveCount = await _context.Drives.AsNoTracking().Where(c => c.IsActive == false && c.Scheduled > now && c.IsDeleted == false).CountAsync();
                    return new ServiceResult<int>(driveCount, "Upcoming drive's count");
                }
                else
                {
                    var now = DateTime.Now;
                    var driveCount = await _appProc.ExecuteStoredProcCollectionAsync<GetTotalCountByExecutiveID_Result>("GetTotalCountByExecutiveID", userId, now, false, false);
                    int count = driveCount.Select(e => e.DriveCount).First();
                    return new ServiceResult<int>(count, "Upcoming Executive's drive count");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Total Candidates Enrolled Count
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> TotalCandidatesEnrolled(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    var driveCount = await _context.Candidates.AsNoTracking().CountAsync();
                    return new ServiceResult<int>(driveCount, "Total Candidate's enrolled count");
                }
                else
                {
                    var now = DateTime.Now;
                    var enrolledCount = await _appProc.ExecuteStoredProcCollectionAsync<GetTotalCountByExecutiveID_Result>("GetTotalCountByExecutiveID", userId, now, false, true);
                    int count = enrolledCount.Select(e => e.DriveCount).First();
                    return new ServiceResult<int>(count, "Total Candidate's enrolled count");
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Total Candidates Enrolled Count on the basis of Date
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<GetCountByDate_Result>>> TotalCandidatesEnrolledByDate(int userId)
        {
            try
            {
                var now = DateTime.Now;
                var todayCount = await _appProc.ExecuteStoredProcCollectionAsync<GetCountByDate_Result>("GetCountByDate", now, "candidate", null);
                return new ServiceResult<IEnumerable<GetCountByDate_Result>>(todayCount, "Total Candidate's enrolled count");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<GetCountByDate_Result>>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Total Drives Created Count on the basis of Date
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<GetCountByDate_Result>>> TotalDriveCreatedByDate(int userId)
        {
            try
            {
                var now = DateTime.Now;
                var todayCount = await _appProc.ExecuteStoredProcCollectionAsync<GetCountByDate_Result>("GetCountByDate", now, "drive", null);
                return new ServiceResult<IEnumerable<GetCountByDate_Result>>(todayCount, "Total Candidate's enrolled count");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<GetCountByDate_Result>>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Category percentage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<GetCategoryPercentBasedOnDrives_Result>>> CategoryPercentByDrives(int userId)
        {
            try
            {
                var list = await _appProc.ExecuteStoredProcCollectionAsync<GetCategoryPercentBasedOnDrives_Result>("GetCategoryPercentBasedOnDrives");
                return new ServiceResult<IEnumerable<GetCategoryPercentBasedOnDrives_Result>>(list, "Total Candidate's enrolled count by Datewise");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<GetCategoryPercentBasedOnDrives_Result>>(ex, ex.Message);
            }
        }
        /// <summary>
        /// Drive percentage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<GetDrivePercentByMonths_Result>>> DrivePercentPerMonth(int userId)
        {
            try
            {
                var list = await _appProc.ExecuteStoredProcCollectionAsync<GetDrivePercentByMonths_Result>("GetDrivePercentByMonths");
                return new ServiceResult<IEnumerable<GetDrivePercentByMonths_Result>>(list, "Total Drives's created count by Datewise");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<GetDrivePercentByMonths_Result>>(ex, ex.Message);
            }
        }
    }

}



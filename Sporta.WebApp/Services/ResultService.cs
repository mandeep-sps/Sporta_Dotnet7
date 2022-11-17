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
    /// Result Service
    /// </summary>
    public class ResultService : IResultService
    {
        private readonly IDriveService _driveService;
        private readonly ISportaRepository<Result> _resultRepo;
        private readonly ISportaRepository<ResultDetail> _resultDetailRepo;
        private readonly IBaseStoredProc _resultProc;
        private readonly ISportaRepository<Candidate> _candidateRepo;
        public ResultService(
            ISportaRepository<Result> resultRepo,
            ISportaRepository<ResultDetail> resultDetailRepo,
            IDriveService driveService, IBaseStoredProc resultProc,
            ISportaRepository<Candidate> candidateRepo)
        {
            _resultRepo = resultRepo;
            _resultDetailRepo = resultDetailRepo;
            _driveService = driveService;
            _resultProc = resultProc;
            _candidateRepo = candidateRepo;
        }


        /// <summary>
        /// Add Candidate Result
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> AddCandidateResult(int userId)
         {
            try
            {
                var drive = await _driveService.GetDriveByCandidate(userId);

                var contextResult = new Result
                {
                    CandidateId = userId,
                    TotalQuestions = drive.Data.Categories.Sum(x => x.CategoryQuestion),
                    Attempted = 0,
                    Score = 0,
                    IsPass = false,
                    IsDeleted = false
                };

                await _resultRepo.AddAsync(contextResult, userId);
                await _resultRepo.SaveChangesAsync();

                foreach (var category in drive.Data.Categories)
                {
                    var contextResultDetail = new ResultDetail
                    {
                        ResultId = contextResult.Id,
                        CategoryId = category.CategoryId,
                        TotalQuestions = category.CategoryQuestion,
                        Attempted = 0,
                        Score = 0,
                        TimeTaken = 0,
                        FocusLost = 0
                    };

                    await _resultDetailRepo.AddAsync(contextResultDetail, userId);
                }

                await _resultRepo.SaveChangesAsync();


                return new ServiceResult<int>(contextResult.Id, "Result Initialized!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Update Candidate Result Detail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> UpdateCandidateResultDetail(TestSubmitRequestModel request, int userId)
        {
            var result = await _resultRepo.GetAsync(x => x.CandidateId == userId);
            var drive = await _driveService.GetDriveByCandidate(userId);
            var contextResultDetail = await _resultDetailRepo.GetAsync(x => x.ResultId == result.Id && x.CategoryId == request.CategoryId);

            contextResultDetail.TotalQuestions = request.TotalQuestions;
            contextResultDetail.Attempted = request.Attemped;
            contextResultDetail.Score = request.Score;
            contextResultDetail.IsAttempted = true;
            contextResultDetail.FocusLost = request.FocusLost;

            contextResultDetail.TimeTaken = request.TimeTaken;

            _resultDetailRepo.Update(contextResultDetail, userId);

            result.Score += request.Score;
            result.Attempted += request.Attemped;
            result.IsPass = Convert.ToDecimal(Convert.ToDecimal(result.Score) / Convert.ToDecimal(result.TotalQuestions)) * 100 >= 50;

            _resultRepo.Update(result, userId);

            if (drive.Data.Categories.All(x => x.IsAttempted))
            {
                var candidate = await _candidateRepo.GetAsync(x => x.Id == userId);
                candidate.CandidateStatusId = 3;
                _candidateRepo.Update(candidate, userId);
            }

            return new ServiceResult<bool>(await _resultRepo.SaveChangesAsync(), "Result Initialized!");
        }


        /// <summary>
        /// Get Result By Candidate
        /// </summary>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ResultResponseModel>> GetResultByCandidate(int candidateId)
        {
            var result = await _resultRepo.GetAsync(x => x.CandidateId == candidateId);
            if (result != null)
                return new ServiceResult<ResultResponseModel>(new ResultResponseModel
                {
                    Id = result.Id,
                    CandidateId = result.CandidateId,
                    TotalQuestions = result.TotalQuestions,
                    Attempted = result.Attempted,
                    Score = result.Score,
                    IsPass = result.IsPass

                }, "Existance Result");

            return new ServiceResult<ResultResponseModel>(null, "Data Not found");
        }


        /// <summary>
        /// Get Report By Drive
        /// </summary>
        /// <param name="driveId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<ReportResponseModel>>> GetReportByDrive(int driveId)
        {
            try
            {
                var list = await _resultProc.ExecuteStoredProcCollectionAsync<GetResultByDrive_Result>("GetResultByDrive", driveId);
                var report = list.Select(x => new ReportResponseModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    UserName = x.UserName,
                    RollNo = x.RollNo,
                    Branch = x.Branch,
                    ContactNumber = x.ContactNumber,
                    TotalQuestions = x.TotalQuestions,
                    Attempted = x.Attempted,
                    Score = x.Score,
                    IsPass = x.IsPass,
                    DriveName = x.DriveName,
                    Status = x.StatusName
                });

                return new ServiceResult<IEnumerable<ReportResponseModel>>(report, "Report List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<ReportResponseModel>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Get result detail by result Id
        /// </summary>
        /// <param name="ResultId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<ReportResponseModel>>> GetResultDetailById(int ResultId)
        {
            try
            {
                var list = await _resultProc.ExecuteStoredProcCollectionAsync<GetResultDetailByResultId_Result>("GetResultDetailByResultId", ResultId);
                var report = list.Select(x => new ReportResponseModel
                {
                    TotalQuestions = x.TotalQuestions,
                    Attempted = x.Attempted,
                    Score = x.Score,
                    CategoryName = x.CategoryName,
                    UserName = x.UserName,
                    Name = x.Name,
                    Branch = x.Branch,
                    ContactNumber = x.ContactNumber,
                    TimeTaken = x.TimeTaken == 0 ? "0" : CommonMethods.ConvertSecondsToMinutes(x.TimeTaken),
                    AllotedTime = x.AllotedTime + " min(s)",
                    FocusLost = x.FocusLost > 1 ? x.FocusLost + " times" : x.FocusLost + " time",

                });

                return new ServiceResult<IEnumerable<ReportResponseModel>>(report, "Report List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<ReportResponseModel>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Get Result Detail By Drive Id
        /// </summary>
        /// <param name="driveId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetResultDetailByDriveId(int driveId)
        {
            try
            {
                var list = await _resultProc.ExecuteStoredProcCollectionAsync<sp_getDetailReportByDriveId_Result>("sp_getDetailReportByDriveId", driveId);

                return new ServiceResult<string>(list?.FirstOrDefault().ResultReport, "Report List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(ex, ex.Message);
            }
        }

    }
}

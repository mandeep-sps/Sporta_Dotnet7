using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporta.WebApp.Services.Interface
{
    /// <summary>
    /// Result Service Interface
    /// </summary>
    public interface IResultService
    {
        /// <summary>
        /// Get Result By Candidate
        /// </summary>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        Task<ServiceResult<ResultResponseModel>> GetResultByCandidate(int candidateId);

        /// <summary>
        /// Add Candidate Result
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<int>> AddCandidateResult(int userId);

        /// <summary>
        /// Update Candidate Result Detail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> UpdateCandidateResultDetail(TestSubmitRequestModel request, int userId);

        /// <summary>
        /// Get Report By Drive
        /// </summary>
        /// <param name="driveId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<ReportResponseModel>>> GetReportByDrive(int driveId);

        /// <summary>
        /// Get Result Detail By Id
        /// </summary>
        /// <param name="ResultId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<ReportResponseModel>>> GetResultDetailById(int ResultId);

        /// <summary>
        /// Get Result Detail By Drive Id
        /// </summary>
        /// <param name="driveId"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> GetResultDetailByDriveId(int driveId);

    }

}

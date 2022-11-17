using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporta.WebApp.Services.Interface
{
    public interface ICandidateService
    {
        /// <summary>
        /// Get Candidates List
        /// </summary>
        /// <returns>Collection of Application Users</returns>
        public Task<ServiceResult<IEnumerable<CandidateResponseModel>>> GetCandidatesByDrive(int driveId);

        /// <summary>
        /// Update Candidate
        /// </summary>
        /// <param name="user"></param>
        /// <param name="modifiedby"></param>
        /// <returns></returns>
        public Task<ServiceResult<string>> UpdateCandidate(ApplicationUserRequestModel user, int modifiedby);
    }
}

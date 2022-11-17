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
    /// Candidate Service
    /// </summary>
    public class CandidateService : ICandidateService
    {
        private readonly ISportaRepository<ApplicationUser> _applictionUserRepo;
        private readonly ISportaRepository<Candidate> _candidateRepo;
        private readonly ISportaRepository<Drive> _driveRepo;
        private readonly ISportaRepository<DriveAssignee> _driveAssigneeRepo;
        public CandidateService(ISportaRepository<ApplicationUser> applictionUserRepo,
            ISportaRepository<Candidate> candidateRepo, ISportaRepository<Drive> driveRepo,
            ISportaRepository<DriveAssignee> driveAssigneeRepo)
        {
            _applictionUserRepo = applictionUserRepo;
            _candidateRepo = candidateRepo;
            _driveAssigneeRepo = driveAssigneeRepo;
            _driveRepo = driveRepo;
        }

        /// <summary>
        /// Get Candidates List
        /// </summary>
        /// <returns> List of Application Users</returns>
        public async Task<ServiceResult<IEnumerable<CandidateResponseModel>>> GetCandidatesByDrive(int driveId)
        {
            try
            {
                var list = await _candidateRepo.GetAllAsync(x => x.DriveId == driveId,
                    D => D.Drive, A => A.IdNavigation, AR => AR.IdNavigation.ApplicationRole);

                var CandidateUsers = list.Select(x => new CandidateResponseModel
                {
                    Id = x.Id,
                    ApplicationRole = x.IdNavigation.ApplicationRole.RoleName,
                    ApplicationRoleId = x.IdNavigation.ApplicationRoleId,
                    Name = x.IdNavigation.Name,
                    Username = x.IdNavigation.Username,
                    Drive = x.Drive.DriveName,
                    DriveId = x.DriveId,
                    ContactNumber = x.ContactNumber,
                    RollNo = x.RollNo,
                    Branch = x.Branch
                });

                return new ServiceResult<IEnumerable<CandidateResponseModel>>(CandidateUsers, "Users List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<CandidateResponseModel>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Update Candidate
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> UpdateCandidate(ApplicationUserRequestModel applicationUser, int modifiedBy)
        {
            try
            {
                var contextModel = await _applictionUserRepo.GetAsync(x => x.Id == applicationUser.Id, a => a.Candidate.Drive);
                if (contextModel != null)
                {
                    if (await _applictionUserRepo.IsExistsAsync(x => x.Candidate.RollNo == applicationUser.CandidateRequestModel.RollNo && x.Candidate.DriveId == applicationUser.CandidateRequestModel.DriveId && x.Id != applicationUser.Id))
                    {
                        return new ServiceResult<string>(null, "Roll No is already registered with " + contextModel.Candidate.Drive.DriveName);
                    }

                    if (await _applictionUserRepo.IsExistsAsync(x => x.Candidate.ContactNumber == applicationUser.CandidateRequestModel.ContactNumber && x.Candidate.DriveId == applicationUser.CandidateRequestModel.DriveId && x.Id != applicationUser.Id))
                    {
                        return new ServiceResult<string>(null, "Contact No. already registered with " + contextModel.Candidate.Drive.DriveName);
                    }
                    else
                    {
                        contextModel.Name = applicationUser.Name;
                        contextModel.Candidate.ContactNumber = applicationUser.CandidateRequestModel.ContactNumber;
                        contextModel.Candidate.RollNo = applicationUser.CandidateRequestModel.RollNo;
                        contextModel.Candidate.Branch = applicationUser.CandidateRequestModel.Branch;

                        _candidateRepo.Update(contextModel.Candidate, modifiedBy);
                        _applictionUserRepo.Update(contextModel, modifiedBy);
                        await _applictionUserRepo.SaveChangesAsync();

                        return new ServiceResult<string>(contextModel.Name, "Candidate updated!");
                    }
                }
                else
                    return new ServiceResult<string>(null, "Candidate not found!", true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(ex, ex.Message);
            }

        }
    }
}

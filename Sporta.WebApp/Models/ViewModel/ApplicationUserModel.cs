using System;

namespace Sporta.WebApp.Models.ViewModel
{
    #region Application User Model Request / Response   

    /// <summary>
    /// Application User Model
    /// </summary>
    public class ApplicationUserModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Application Role Id
        /// </summary>
        public int ApplicationRoleId { get; set; }

        /// <summary>
        /// Application Role
        /// </summary>
        public string ApplicationRole { get; set; }

        /// <summary>
        /// Candidate Request Model
        /// </summary>
        public CandidateRequestModel CandidateRequestModel { get; set; }
    }


    /// <summary>
    /// Application User Request Model
    /// </summary>
    public class ApplicationUserRequestModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email Id
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// User Password
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Application Role Id
        /// </summary>
        public int ApplicationRoleId { get; set; }

        /// <summary>
        /// Candidate Request Model
        /// </summary>
        public CandidateRequestModel CandidateRequestModel { get; set; }

    }

    #endregion


    #region Candidate Model Request / Response   

    /// <summary>
    /// Candidate Request Model
    /// </summary>
    public class CandidateRequestModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Contact Number
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public int Token { get; set; }

        /// <summary>
        /// Roll No
        /// </summary>
        public string RollNo { get; set; }

        /// <summary>
        /// Branch
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Drive Id
        /// </summary>
        public int DriveId { get; set; }

    }

    /// <summary>
    /// Candidate Response Model
    /// </summary>
    public class CandidateResponseModel : CandidateRequestModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Application Role Id
        /// </summary>
        public int ApplicationRoleId { get; set; }

        /// <summary>
        /// Application Role
        /// </summary>
        public string ApplicationRole { get; set; }

        /// <summary>
        /// Drive
        /// </summary>
        public string Drive { get; set; }

    }

    #endregion


    #region Helper Class  

    /// <summary>
    /// Drive Dropdown
    /// </summary>
    public class DriveDropDown
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Drive Name
        /// </summary>
        public string DriveName { get; set; }

    }

    /// <summary>
    /// Active Executives Details
    /// </summary>
    public class ActiveExecutiveResponseModel
    {

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Executive Username
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Drive Name
        /// </summary>
        public string DriveName { get; set; }
    }
    #endregion
}


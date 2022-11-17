using System;
using System.Collections.Generic;

namespace Sporta.WebApp.Models.ViewModel
{
    #region Drive Model Request / Response / Filter   


    /// <summary>
    /// Drive Request Model
    /// </summary>
    public class DriveRequestModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Drive Name
        /// </summary>
        public string DriveName { get; set; }

        /// <summary>
        /// Scheduled
        /// </summary>
        public DateTime Scheduled { get; set; }

        /// <summary>
        /// Alloted Time
        /// </summary>
        public int AllotedTime { get; set; }

        /// <summary>
        /// Total Question
        /// </summary>
        public int TotalQuestion { get; set; }

        /// <summary>
        /// Drive Categories
        /// </summary>
        public IEnumerable<DriveCategoryModel> DriveCategories { get; set; }

        /// <summary>
        /// Drive Assignee
        /// </summary>
        public IEnumerable<int> DriveAssignee { get; set; }
    }

    /// <summary>
    /// Drive Response Model
    /// </summary>
    public class DriveResponseModel : DriveRequestModel
    {
        /// <summary>
        /// Drive Category
        /// </summary>
        public IEnumerable<DriveCategoryModel> DriveCategory { get; set; }

        /// <summary>
        /// Categories
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        /// Assignees
        /// </summary>
        public string Assignees { get; set; }

        /// <summary>
        /// Enrolled
        /// </summary>
        public int Enrolled { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public int Token { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public bool Status { get; set; }


        /// <summary>
        /// Question Ids
        /// </summary>
        public string QuestionIds { get; set; }
    }

    /// <summary>
    /// Drive Filter Request Model
    /// </summary>
    public class DriveFilterRequestModel
    {
        /// <summary>
        /// Drive Id
        /// </summary>
        public int DriveId { get; set; }

        /// <summary>
        /// Drive Name
        /// </summary>
        public string DriveName { get; set; }

        /// <summary>
        /// From Date
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// To Date
        /// </summary>
        public DateTime? ToDate { get; set; }

    }

    public class DriveFilterDashboard
    {
        /// <summary>
        /// IsLive
        /// </summary>
        public bool isLive { get; set; }

        /// <summary>
        /// Datetime
        /// </summary>
        public bool UpcomingDrive { get; set; }

        /// <summary>
        /// Datetime
        /// </summary>
        public bool Scheduled { get; set; }
    }
    #endregion


    #region Candidate Drive Model Request / Response   

    /// <summary>
    /// Candidate Drive Reponse Model
    /// </summary>
    public class CandidateDriveReponseModel
    {
        /// <summary>
        /// Drive Id
        /// </summary>
        public int DriveId { get; set; }

        /// <summary>
        /// Drive Name
        /// </summary>
        public string DriveName { get; set; }

        /// <summary>
        /// Scheduled
        /// </summary>
        public DateTime Scheduled { get; set; }

        /// <summary>
        /// Categories
        /// </summary>
        public IEnumerable<DriveCategoryModel> Categories { get; set; }
    }

    #endregion


    #region Drive Category Model Request / Response   

    /// <summary>
    /// Drive Category Model
    /// </summary>
    public class DriveCategoryModel
    {
        /// <summary>
        /// Category Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Category Time
        /// </summary>
        public int CategoryTime { get; set; }

        /// <summary>
        /// Category Question
        /// </summary>
        public int CategoryQuestion { get; set; }

        /// <summary>
        /// Category Question
        /// </summary>
        public string SelectedQuestions { get; set; }

        /// <summary>
        /// Is Attempted
        /// </summary>
        public bool IsAttempted { get; set; }

    }

    #endregion


    #region Executive Drive Model Request / Response

    /// <summary>
    /// Executive Drive Response Model
    /// </summary>
    public class ExecutiveDriveResponseModel
    {
        /// <summary>
        /// Executive Id
        /// </summary>
        public int ExecutiveId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string ExecutiveName { get; set; }

        /// <summary>
        /// Drive Id
        /// </summary>
        public int DriveId { get; set; }

        /// <summary>
        /// Drive Name
        /// </summary>
        public string DriveName { get; set; }

        /// <summary>
        /// Enrolled Count
        /// </summary>
        public int EnrolledCount { get; set; }

        /// <summary>
        /// Scheduled
        /// </summary>
        public DateTime ScheduledTime { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public int Token { get; set; }


    }

    #endregion

}

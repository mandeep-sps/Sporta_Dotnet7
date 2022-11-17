namespace Sporta.WebApp.Models.ViewModel
{

    #region Result Model Request / Response 

    /// <summary>
    /// Result Response Model
    /// </summary>
    public class ResultResponseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Candidate Id
        /// </summary>
        public int CandidateId { get; set; }

        /// <summary>
        /// Total Questions
        /// </summary>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// Attempted
        /// </summary>
        public int Attempted { get; set; }

        /// <summary>
        /// Score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Is Pass
        /// </summary>
        public bool IsPass { get; set; }
    }

    #endregion


    #region Report Model Request / Response 

    /// <summary>
    /// Report Response Model
    /// </summary>
    public class ReportResponseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Roll No
        /// </summary>
        public string RollNo { get; set; }

        /// <summary>
        /// Branch
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Contact Number
        /// </summary>
        public string ContactNumber { get; set; }
        /// <summary>
        /// Total Questions
        /// </summary>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// Attempted
        /// </summary>
        public int Attempted { get; set; }

        /// <summary>
        /// Score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Is Pass
        /// </summary>
        public bool IsPass { get; set; }

        /// <summary>
        /// Drive Name
        /// </summary>
        public string DriveName { get; set; }

        /// <summary>
        /// Category Name
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Alloted Time
        /// </summary>
        public string AllotedTime { get; set; }

        /// <summary>
        /// Time Taken
        /// </summary>
        public string TimeTaken { get; set; }

        /// <summary>
        /// Is Focus Lost
        /// </summary>
        public string FocusLost { get; set; }
    }

    #endregion
}

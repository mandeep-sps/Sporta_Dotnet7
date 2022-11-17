using System;
using System.Collections.Generic;

namespace Sporta.WebApp.Models.ViewModel
{
    #region Question Model Request / Response 

    /// <summary>
    /// Question Response Model
    /// </summary>
    public class QuestionResponseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Question Name
        /// </summary>
        public string Question1 { get; set; }

        /// <summary>
        /// Option A
        /// </summary>
        public string OptionA { get; set; }

        /// <summary>
        /// Option B
        /// </summary>
        public string OptionB { get; set; }

        /// <summary>
        /// Option C
        /// </summary>
        public string OptionC { get; set; }

        /// <summary>
        /// Option D
        /// </summary>
        public string OptionD { get; set; }

        /// <summary>
        /// Answer
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Category Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Question Level
        /// </summary>
        public string QuestionLevel { get; set; }
    }

    /// <summary>
    /// Question Request Model
    /// </summary>
    public class QuestionRequestModel : QuestionResponseModel
    {
        /// <summary>
        /// Is Active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Created On
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Modified By
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Modified On
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Is Deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        ///// <summary>
        ///// Question Level
        ///// </summary>
        //public string QuestionLevel { get; set; }

    }

    #endregion


    #region Category Model Request / Response 

    /// <summary>
    /// Category Model
    /// </summary>
    public class CategoryModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Category Name
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Allocated Questions
        /// </summary>
        public string AllocatedQuestions { get; set; }
    }

    #endregion


    #region Test Questions Model Request / Response

    /// <summary>
    /// Test Questions Response Model
    /// </summary>
    public class TestQuestionsResponseModel
    {
        /// <summary>
        /// Questions
        /// </summary>
        public IEnumerable<QuestionResponseModel> Questions { get; set; }

        /// <summary>
        /// Result Id
        /// </summary>
        public int ResultId { get; set; }

        /// <summary>
        /// Drive Id
        /// </summary>
        public int DriveId { get; set; }

        /// <summary>
        /// Alloted Time
        /// </summary>
        public int AllotedTime { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Category Id
        /// </summary>
        public int CategoryId { get; set; }
    }

    #endregion


    #region Test Submit Model Request / Response 

    /// <summary>
    /// Test Submit Request Model
    /// </summary>
    public class TestSubmitRequestModel
    {
        /// <summary>
        /// Drive Id
        /// </summary>
        public int DriveId { get; set; }

        /// <summary>
        /// Result Id
        /// </summary>
        public int ResultId { get; set; }

        /// <summary>
        /// Category Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Total Questions
        /// </summary>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// Attemped
        /// </summary>
        public int Attemped { get; set; }

        /// <summary>
        /// Score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Time Taken
        /// </summary>
        public long TimeTaken { get; set; }

        /// <summary>
        /// Is Focus Lost
        /// </summary>
        public int FocusLost { get; set; }
    }

    #endregion
}

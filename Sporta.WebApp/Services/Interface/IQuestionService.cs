using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sporta.Data.Database.Sporta;

namespace Sporta.WebApp.Services.Interface
{
    /// <summary>
    /// Question Service Interface
    /// </summary>
    public interface IQuestionService
    {
        #region Question Service

        /// <summary>
        /// Get All Questions
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<QuestionResponseModel>>> GetAllQuestions();

        /// <summary>
        /// Get Questions By Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>

        Task<ServiceResult<IEnumerable<QuestionResponseModel>>> GetQuestionsByCategory(int categoryId);

        /// <summary>
        /// Get Question Name By Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>

        Task<ServiceResult<IEnumerable<string>>> GetQuestionNameByCategory(int categoryId);

        /// <summary>
        /// Add Bulk Question
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createdBy"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>

        Task<ServiceResult<bool>> AddBulkQuestion(List<QuestionRequestModel> request, int createdBy, int categoryId);

        /// <summary>
        /// Add Question
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        Task<ServiceResult<int>> AddQuestion(QuestionRequestModel request, int createdBy);

        /// <summary>
        /// Get Question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<QuestionResponseModel>> GetQuestion(int id);

        /// <summary>
        /// Update Question
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<int>> UpdateQuestion(QuestionRequestModel request, int modifiedBy);

        /// <summary>
        /// Remove Question
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> RemoveQuestion(int Id, int modifiedBy);


        /// <summary>
        /// Remove Question
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<Question>>> GetQuestionListByLevels(int categoryId, bool isHard, bool isEasy, bool isMedium);
        #endregion


        #region Caregory Service

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<CategoryModel>>> GetAllCategories();

        /// <summary>
        /// Get Categories DropDown
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<DropdownModel>>> GetCategoriesDropDown();

        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> AddCategory(CategoryModel request, int createdBy);

        /// <summary>
        /// Get Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ServiceResult<CategoryModel>> GetCategory(int id);

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> UpdateCategory(CategoryModel request, int modifiedBy);

        /// <summary>
        /// Remove Category
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> RemoveCategory(int Id, int modifiedBy);

        #endregion


        #region Test Screen Functions

        /// <summary>
        /// Get Drive Category Questions
        /// </summary>
        /// <param name="driveId"></param>
        /// <param name="categoryId"></param>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        Task<ServiceResult<TestQuestionsResponseModel>> GetDriveCategoryQuestions(int driveId, int categoryId, int candidateId);

        #endregion
    }
}

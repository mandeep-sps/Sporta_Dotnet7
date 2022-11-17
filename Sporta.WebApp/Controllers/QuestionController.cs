using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporta.WebApp.Controllers
{
    /// <summary>
    /// Question Controller
    /// </summary>
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _questionService;
        private readonly IGeneralService _generalService;
        public QuestionController(IQuestionService questionService, IGeneralService generalService)
        {
            _questionService = questionService;
            _generalService = generalService;
        }


        #region Question Views

        /// <summary>
        /// Index View
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1,2")]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Category CRUD Actions

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns></returns>
        public async Task<PartialViewResult> GetAllCategories()
        {
            var response = await _questionService.GetAllCategories();
            return PartialView("_GetAllCategoriesPartial", response.Data);
        }

        /// <summary>
        /// Get Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCategory(int id)
        {
            return GetResult(await _questionService.GetCategory(id));
        }

        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddCategory(CategoryModel request)
        {
            return GetResult(await _questionService.AddCategory(request, User.GetUserId()));
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateCategory(CategoryModel request)
        {
            return GetResult(await _questionService.UpdateCategory(request, User.GetUserId()));
        }

        /// <summary>
        /// Remove Category
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemoveCategory(int Id)
        {
            return GetResult(await _questionService.RemoveCategory(Id, User.GetUserId()));
        }

        #endregion

        #region Questions CRUD Actions

        /// <summary>
        /// Get Questions
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetQuestions()
        {
            return GetResult(await _questionService.GetAllQuestions());
        }

        /// <summary>
        /// Get Categories Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetCategoriesDropdown()
        {
            return GetResult(await _questionService.GetCategoriesDropDown());
        }

        /// <summary>
        /// Get Question Name By Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetQuestionNameByCategory(int categoryId)
        {
            return GetResult(await _questionService.GetQuestionNameByCategory(categoryId));
        }

        /// <summary>
        /// Get Questions By Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetQuestionsByCategory(int categoryId)
        {
            return GetResult(await _questionService.GetQuestionsByCategory(categoryId));
        }

        /// <summary>
        /// Add Question
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResult> AddQuestion(QuestionRequestModel request)
        {
            return GetResult(await _questionService.AddQuestion(request, User.GetUserId()));
        }

        /// <summary>
        /// Add Bulk Question
        /// </summary>
        /// <param name="request"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AddBulkQuestion(List<QuestionRequestModel> request, int categoryId)
        {
            return GetResult(await _questionService.AddBulkQuestion(request, User.GetUserId(), categoryId));
        }

        /// <summary>
        /// Get Question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetQuestion(int id)
        {
            return GetResult(await _questionService.GetQuestion(id));
        }

        /// <summary>
        /// Update Question
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateQuestion(QuestionRequestModel request)
        {
            return GetResult(await _questionService.UpdateQuestion(request, User.GetUserId()));
        }

        /// <summary>
        /// Remove Question
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemoveQuestion(int Id)
        {
            return GetResult(await _questionService.RemoveQuestion(Id, User.GetUserId()));
        }

        /// <summary>
        /// Get Questions By Levels
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetQuestionListByLevels(int categoryId, bool isEasy, bool isMedium, bool isHard)
        {
            return GetResult(await _questionService.GetQuestionListByLevels(categoryId, isEasy, isMedium, isHard));
        }

        #endregion

    }
}

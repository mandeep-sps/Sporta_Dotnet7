using Microsoft.EntityFrameworkCore;
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
    /// Question Service
    /// </summary>
    public class QuestionService : IQuestionService    
    {
        private readonly ISportaRepository<Question> _questRepo;
        private readonly ISportaRepository<Category> _categoryRepo;
        private readonly ISportaRepository<Candidate> _candidateRepo;
        private readonly Sporta_DbContext _context;
        public QuestionService(ISportaRepository<Question> questRepo, ISportaRepository<Candidate> candidateRepo, ISportaRepository<Category> categoryRepo, Sporta_DbContext context)
        {
            _questRepo = questRepo;
            _context = context;
            _categoryRepo = categoryRepo;
            _candidateRepo = candidateRepo;
        }


        #region Category Service

        /// <summary>
        /// Get Categories DropDown
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<DropdownModel>>> GetCategoriesDropDown()
        {
            try
            {
                var list = await _categoryRepo.GetAllAsync();
                var dropDown = list.OrderBy(o => o.CategoryName).Select(x => new DropdownModel
                {
                    Id = x.Id,
                    Name = x.CategoryName

                });

                return new ServiceResult<IEnumerable<DropdownModel>>(dropDown, "Dropdown List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<DropdownModel>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<CategoryModel>>> GetAllCategories()
        {
            try
            {
                var list = await _categoryRepo.GetAllAsync(q => q.Questions.Where(a => !a.IsDeleted));
                var categories = list.Select(x => new CategoryModel
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName,
                    AllocatedQuestions = x.Questions.Count > 1 ? x.Questions.Count + " questions allocated. " : x.Questions.Count + " question allocated."
                });

                return new ServiceResult<IEnumerable<CategoryModel>>(categories, "Categories List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<CategoryModel>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> AddCategory(CategoryModel request, int createdBy)
        {
            try
            {
                if (await _categoryRepo.IsExistsAsync(x => x.CategoryName.ToLower() == request.CategoryName.Trim().ToLower()))
                {
                    return new ServiceResult<bool>(false, $"{request.CategoryName} is already exist", true);
                }

                var category = new Category
                {
                    CategoryName = request.CategoryName.ToUpper(),
                    IsDeleted = false
                };
                await _categoryRepo.AddAsync(category, createdBy);

                return new ServiceResult<bool>(await _categoryRepo.SaveChangesAsync(), "Category Added Successfully!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Get Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<CategoryModel>> GetCategory(int id)
        {
            try
            {
                var category = await _categoryRepo.GetAsync(x => x.Id == id);

                if (category != null)
                {
                    CategoryModel returnModel = new()
                    {
                        CategoryName = category.CategoryName,
                        Id = category.Id
                    };

                    return new ServiceResult<CategoryModel>(returnModel, "Category Detail");
                }

                return new ServiceResult<CategoryModel>(null, "Category doesn't exist", true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<CategoryModel>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> UpdateCategory(CategoryModel request, int modifiedBy)
        {
            try
            {
                if (await _categoryRepo.IsExistsAsync(x => x.CategoryName.ToLower() == request.CategoryName.Trim().ToLower() && x.Id != request.Id))
                {
                    return new ServiceResult<bool>(false, $"{request.CategoryName} is already exist!", true);
                }

                var category = await _categoryRepo.GetAsync(x => x.Id == request.Id);

                if (category != null)
                {
                    category.CategoryName = request.CategoryName.ToUpper();

                    _categoryRepo.Update(category, modifiedBy);

                    return new ServiceResult<bool>(await _categoryRepo.SaveChangesAsync(), "Category updated Successfully!");
                }

                return new ServiceResult<bool>(false, "Category doesn't exist", true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Remove Category
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> RemoveCategory(int Id, int modifiedBy)
        {
            try
            {
                var category = await _categoryRepo.GetAsync(x => x.Id == Id, a => a.Questions);
                if (category.Questions.Count > 0)
                {
                    var count = category.Questions.Count > 1 ? category.Questions.Count + " questions " : category.Questions.Count + " question";
                    return new ServiceResult<bool>(false, $"Category has {count} ascociated, Can't delete Category.", true);
                }

                await _categoryRepo.Remove(Id, modifiedBy);
                return new ServiceResult<bool>(await _questRepo.SaveChangesAsync(), "Category deleted Successfully!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }

        #endregion


        #region Question Service

        /// <summary>
        /// Get All Questions
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QuestionResponseModel>>> GetAllQuestions()
        {
            try
            {
                var list = await _questRepo.GetAllAsync(x => x.Category);
                var questions = list.Select(x => new QuestionResponseModel
                {
                    Id = x.Id,
                    Question1 = x.Question1,
                    OptionA = x.OptionA,
                    OptionB = x.OptionB,
                    OptionC = x.OptionC,
                    OptionD = x.OptionD,
                    Answer = x.Answer,
                    QuestionLevel = x.QuestionLevel,
                    CategoryId = x.CategoryId,
                    Category = x.Category.CategoryName
                });

                return new ServiceResult<IEnumerable<QuestionResponseModel>>(questions, "Questions List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<QuestionResponseModel>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Get Questions By Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QuestionResponseModel>>> GetQuestionsByCategory(int categoryId)
        {
            try
            {
                var list = await _questRepo.GetAllAsync(x => x.CategoryId == categoryId, a => a.Category);

                var questions = list.Select(x => new QuestionResponseModel
                {
                    Id = x.Id,
                    Question1 = x.Question1,
                    OptionA = x.OptionA,
                    OptionB = x.OptionB,
                    OptionC = x.OptionC,
                    OptionD = x.OptionD,
                    Answer = x.Answer,
                    QuestionLevel = x.QuestionLevel,
                    CategoryId = x.CategoryId,
                    Category = x.Category.CategoryName
                });

                return new ServiceResult<IEnumerable<QuestionResponseModel>>(questions, "question List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<QuestionResponseModel>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Get Question Name By Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<string>>> GetQuestionNameByCategory(int categoryId)
        {
            try
            {
                var list = await _questRepo.GetAllAsync(x => x.CategoryId == categoryId);

                var questions = list.Select(x => x.Question1.ToLower());

                return new ServiceResult<IEnumerable<string>>(questions, "question name List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<string>>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Add Bulk Question
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createdBy"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> AddBulkQuestion(List<QuestionRequestModel> request, int createdBy, int categoryId)
        {
            try
            {
                var questions = new List<Question>();
                foreach (var item in request)
                {
                    var questionSet = new Question
                    {
                        Question1 = item.Question1,
                        OptionA = item.OptionA,
                        OptionB = item.OptionB,
                        OptionC = item.OptionC,
                        OptionD = item.OptionD,
                        Answer = item.Answer,
                        QuestionLevel = item.QuestionLevel,
                        CategoryId = categoryId,
                        IsActive = true,
                        ModifiedBy = createdBy,
                        ModifiedOn = DateTime.Now,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };

                    questions.Add(questionSet);
                }
                await _context.Questions.AddRangeAsync(questions);
                await _context.SaveChangesAsync();


                return new ServiceResult<bool>(true, "Questions Added Successfully!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Add Question
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> AddQuestion(QuestionRequestModel request, int createdBy)
        {
            try
            {
                if (await _questRepo.IsExistsAsync(x => x.Question1.ToLower() == request.Question1.ToLower() && x.CategoryId == request.CategoryId))
                {
                    return new ServiceResult<int>(-1, "Questions already exist in current category!", true);
                }

                var questionSet = new Question
                {
                    Question1 = request.Question1,
                    OptionA = request.OptionA,
                    OptionB = request.OptionB,
                    OptionC = request.OptionC,
                    OptionD = request.OptionD,
                    Answer = request.Answer,
                    QuestionLevel = request.QuestionLevel,
                    CategoryId = request.CategoryId,
                    IsActive = true,
                };

                await _questRepo.AddAsync(questionSet, createdBy);
                await _questRepo.SaveChangesAsync();

                return new ServiceResult<int>(questionSet.CategoryId, "Questions Added Successfully!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Update Question
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<int>> UpdateQuestion(QuestionRequestModel request, int modifiedBy)
        {
            try
            {
                if (await _questRepo.IsExistsAsync(x => x.Question1.ToLower() == request.Question1.ToLower() && x.CategoryId == request.CategoryId && x.Id != request.Id))
                {
                    return new ServiceResult<int>(-1, "Questions already exist in current category!", true);
                }

                var contextModel = await _questRepo.GetAsync(x => x.Id == request.Id);

                if (contextModel != null)
                {
                    contextModel.Question1 = request.Question1;
                    contextModel.OptionA = request.OptionA;
                    contextModel.OptionB = request.OptionB;
                    contextModel.OptionC = request.OptionC;
                    contextModel.OptionD = request.OptionD;
                    contextModel.Answer = request.Answer;
                    contextModel.QuestionLevel = request.QuestionLevel;
                    contextModel.CategoryId = request.CategoryId;

                    _questRepo.Update(contextModel, modifiedBy);
                    await _questRepo.SaveChangesAsync();

                    return new ServiceResult<int>(contextModel.CategoryId, "Question updated successfully!");
                }

                return new ServiceResult<int>(-1, "Question not found!", true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(ex, ex.Message);
            }

        }


        /// <summary>
        /// Get Question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<QuestionResponseModel>> GetQuestion(int id)
        {
            try
            {
                var question = await _questRepo.GetAsync(x => x.Id == id);
                QuestionResponseModel response = new QuestionResponseModel();
                if (question != null)
                {
                    response.Id = question.Id;
                    response.Question1 = question.Question1;
                    response.OptionA = question.OptionA;
                    response.OptionB = question.OptionB;
                    response.OptionC = question.OptionC;
                    response.OptionD = question.OptionD;
                    response.Answer = question.Answer;
                    response.QuestionLevel = question.QuestionLevel;
                    response.CategoryId = question.CategoryId;
                }
                return new ServiceResult<QuestionResponseModel>(response, null);
            }
            catch (Exception ex)
            {
                return new ServiceResult<QuestionResponseModel>(ex, ex.Message);
            }
        }


        /// <summary>
        /// Remove Question
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> RemoveQuestion(int Id, int modifiedBy)
        {
            try
            {
                await _questRepo.Remove(Id, modifiedBy);
                return new ServiceResult<bool>(await _questRepo.SaveChangesAsync(), "Question deleted Successfully!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }

        }


        #endregion


        #region Test Screeen Functions

        /// <summary>
        /// Get Drive Category Questions
        /// </summary>
        /// <param name="driveId"></param>
        /// <param name="categoryId"></param>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<TestQuestionsResponseModel>> GetDriveCategoryQuestions(int driveId, int categoryId, int candidateId)
        {
            try
            {
                var category = await _candidateRepo.GetAsync(x => x.Id == candidateId);
                category.CandidateStatusId = 2;
                _candidateRepo.Update(category, candidateId);
                await _candidateRepo.SaveChangesAsync();
                var driveCategory = await _context.DriveCategories
                                        .Where(x => x.DriveId == driveId && x.CategoryId == categoryId)
                                        .Include(C => C.Category)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();

                //Get Questions From driveCategory
                string[] qIds = driveCategory.QuestionIds.Split(',');
                int[] questionIds = Array.ConvertAll(qIds, s => int.Parse(s));

                var list = await _context.Questions
                                   .Where(x => x.CategoryId == categoryId && !x.IsDeleted && questionIds.Contains(x.Id))
                                   .OrderBy(x => Guid.NewGuid()) //To get random rows
                                   .Take(driveCategory.TotalQuestion)
                                   .AsNoTracking()
                                   .ToListAsync();

                

                var result = await _context.Results.Where(x => x.CandidateId == candidateId).FirstOrDefaultAsync();
                var response = new TestQuestionsResponseModel
                {
                    AllotedTime = Convert.ToInt32(driveCategory.AllotedTime),
                    ResultId = result == null ? 0 : result.Id,
                    Category = driveCategory.Category.CategoryName,
                    CategoryId = driveCategory.Category.Id,
                    DriveId = driveId,
                    Questions = list.Select(x => new QuestionResponseModel
                    {
                        Id = x.Id,
                        Question1 = x.Question1,
                        OptionA = x.OptionA,
                        OptionB = x.OptionB,
                        OptionC = x.OptionC,
                        OptionD = x.OptionD,
                        Answer = x.Answer,
                    }).OrderBy(x => Guid.NewGuid())
                };

                return new ServiceResult<TestQuestionsResponseModel>(response, "Test Questions");
            }
            catch (Exception ex)
            {
                return new ServiceResult<TestQuestionsResponseModel>(ex, ex.Message);
            }
        }

        /// <summary>
        /// Get Question List By Category Id and Levelwise
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<Question>>> GetQuestionListByLevels(int categoryId, bool isEasy, bool isMedium, bool isHard)
        {
            try
            {
                IEnumerable<Question> list = new List<Question>();
                if (isEasy)
                {
                    list = await _questRepo.GetAllAsync(x => x.CategoryId == categoryId && !x.IsDeleted && x.QuestionLevel == "Easy");
                }
                else if (isMedium)
                {
                    list = await _questRepo.GetAllAsync(x => x.CategoryId == categoryId && !x.IsDeleted && x.QuestionLevel == "Medium");
                }
                else
                {
                    list = await _questRepo.GetAllAsync(x => x.CategoryId == categoryId && !x.IsDeleted && x.QuestionLevel == "Hard");
                }
                //QuestionResponseModel response = new QuestionResponseModel();
                //if (list != null)
                //{
                    var response = list.Select(x => new QuestionResponseModel
                    {
                        Id = x.Id,
                        Question1 = x.Question1,
                        Category = x.Category.CategoryName
                    });

                    return new ServiceResult<IEnumerable<Question>>(list, "questions List!");
                //}
                //return new ServiceResult<IEnumerable<QuestionResponseModel>>(null, "questions List!");
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<Question>>(ex, ex.Message);
            }
        }


        #endregion

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sporta.WebApp.Common;
using Sporta.WebApp.Models.ViewModel;
using Sporta.WebApp.Services.Interface;
using System;
using System.Threading.Tasks;

namespace Sporta.WebApp.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        private readonly IGeneralService _generalService;
        public ErrorController(IGeneralService generalService)
        {
            _generalService = generalService;
        }


        /// <summary>
        /// Exception
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        [Route("Exception")]
        public async Task<IActionResult> Exception()
        {
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            int statusCode = error is HttpResponseException ? (error as HttpResponseException).Status : HttpContext.Response.StatusCode;

            if (statusCode == 500)
            {
                ViewBag.ErrorCode = (await LogErrorAsync(error)).ToString().ToUpper();
                HttpContext.Response.StatusCode = 500;
            }

            return statusCode switch
            {
                404 => RedirectToAction(nameof(PageNotFound)),
                500 => View("InternalServerPage"),
                _ => View("InternalServerPage")
            };
        }


        /// <summary>
        /// Page Not Found
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("404")]
        public IActionResult PageNotFound()
        {
            HttpContext.Response.StatusCode = 404;
            return View();
        }

        /// <summary>
        /// Session Expired
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("SessionExpired")]
        public IActionResult SessionExpired()
        {
            return View("ExceptionPage");
        }

        public async Task<Guid> LogErrorAsync(Exception error = null)
        {
            ErrorRequest request = new()
            {
                Information = $"Exception Message: {error.Message}, Exception Stack Trace: {error.StackTrace}",
                UserId = User.GetUserId()
            };

            return (await _generalService.CreateErrorLog(request)).Data;
        }

    }
}

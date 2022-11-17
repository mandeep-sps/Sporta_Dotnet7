using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sporta.WebApp.Common;

namespace Sporta.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Index View
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.GetUserRole() == (int)UserRole.Admin || User.GetUserRole() == (int)UserRole.Executive)
                    return RedirectToAction("Index", "Drive");

                else if (User.GetUserRole() == (int)UserRole.Candidate)
                    return RedirectToAction("Index", "Candidate");

                else
                    return View();
            }
            return View();
        }

        /// <summary>
        /// Privacy
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

    }
}

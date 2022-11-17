using Microsoft.AspNetCore.Hosting;

namespace Sporta.WebApp.Common
{
    public class Utils
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public Utils(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public string GetEnvironment()
        {
            return _hostEnvironment.EnvironmentName;
        }
    }
}

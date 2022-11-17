using System;

namespace Sporta.WebApp.Common
{
    /// <summary>
    /// Http Response Exception
    /// </summary>
    public class HttpResponseException : Exception
    {
        public HttpResponseException(int status)
        {
            Status = status;
        }

        /// <summary>
        ///     Status
        /// </summary>
        public int Status { get; set; }
    }
}

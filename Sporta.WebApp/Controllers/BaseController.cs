using Microsoft.AspNetCore.Mvc;
using Sporta.WebApp.Common;
using System;

namespace Sporta.WebApp.Controllers
{
    /// <summary>
    /// Base Controller
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Base Response
        /// </summary>
        public class BaseResponse
        {
            /// <summary>
            /// Data
            /// </summary>
            public dynamic Data { get; set; }

            /// <summary>
            /// Exception
            /// </summary>
            public Exception Exception { get; set; }

            /// <summary>
            /// Message
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Is Success
            /// </summary>
            public bool IsSuccess { get; set; }
        }

        /// <summary>
        /// Get Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceResult"></param>
        /// <returns></returns>
        public JsonResult GetResult<T>(ServiceResult<T> serviceResult)
        {
            return Json(new BaseResponse
            {
                Data = serviceResult.Data,
                Exception = serviceResult.Exception,
                Message = serviceResult.Message,
                IsSuccess = serviceResult.IsSuccess
            });

        }
    }
}

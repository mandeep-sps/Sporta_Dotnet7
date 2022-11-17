namespace Sporta.WebApp.Models
{
    /// <summary>
    /// Error View Model
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Request Id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Show Request Id
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

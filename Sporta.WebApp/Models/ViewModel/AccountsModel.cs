namespace Sporta.WebApp.Models.ViewModel
{
    #region Login Model Request / Response   

    /// <summary>
    /// Login View Model
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Email Id
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// User Password
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Is From Signup
        /// </summary>
        public bool IsFromSignup { get; set; } = false;

    }


    /// <summary>
    /// Login Response Model
    /// </summary>
    public class LoginResponseModel
    {
        /// <summary>
        /// User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Role Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Response Type
        /// </summary>
        public string ResponseType { get; set; }

    }

    #endregion
}

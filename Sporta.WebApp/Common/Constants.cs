using System.Security.Claims;

namespace Sporta.WebApp.Common
{
    /// <summary>
    /// Application Claim Types for Auth
    /// </summary>
    public sealed class AppClaimTypes
    {
        /// <summary>
        /// User Id
        /// </summary>
        public const string UserId = nameof(UserId);

        /// <summary>
        /// Role
        /// </summary>
        public const string Role = ClaimTypes.Role;

        /// <summary>
        /// Name
        /// </summary>
        public const string Name = nameof(Name);

        /// <summary>
        /// User Name / Email
        /// </summary>
        public const string UserName = nameof(UserName);

    }
}

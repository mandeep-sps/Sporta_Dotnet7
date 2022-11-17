using System.Linq;
using System.Security.Claims;

namespace Sporta.WebApp.Common
{
    /// <summary>
    /// Pricipal Extension Class
    /// </summary>
    public static class PricipalExtension
    {
        /// <summary>
        /// Get Username / Email Extension
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>Username / Email of Signed in user</returns>
        public static string GetUsername(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == AppClaimTypes.UserName)?.Value;
        }

        /// <summary>
        /// Get User Role Extension
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>User Role of Signed in user</returns>
        public static int GetUserRole(this ClaimsPrincipal principal)
        {
            if (int.TryParse(principal?.Claims?.FirstOrDefault(x => x.Type == AppClaimTypes.Role)?.Value, out int roleId))
                return roleId;
            return -1;
        }

        /// <summary>
        /// Get User Id Extension
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>User Id of Signed in user</returns>
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (int.TryParse(principal?.Claims?.FirstOrDefault(x => x.Type == AppClaimTypes.UserId)?.Value, out int id))
                return id;
            return -1;
        }

        /// <summary>
        /// Get Name Extension
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>Name of Signed in user</returns>
        public static string GetName(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == AppClaimTypes.Name)?.Value;
        }
    }
}

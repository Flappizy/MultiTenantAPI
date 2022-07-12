using AuthPermissions;
using AuthPermissions.AdminCode;
using System.Security.Claims;

namespace PLCubsWebAPI.Services
{
    public class AddTenantNameClaim : IClaimsAdder
    {
        public const string TenantNameClaimType = "TenantName";

        //This provide you CRUD functionanlity to Teanant users
        private readonly IAuthUsersAdminService _userAdmin;

        public AddTenantNameClaim(IAuthUsersAdminService userAdmin)
        {
            _userAdmin = userAdmin;
        }

        //This is used to add Tenant name to a user's claim
        public async Task<Claim> AddClaimToUserAsync(string userId)
        {
            var user = (await _userAdmin.FindAuthUserByUserIdAsync(userId)).Result;

            return user?.UserTenant?.TenantFullName == null
                ? null
                : new Claim(TenantNameClaimType, user.UserTenant.TenantFullName);
        }

        public static string GetTenantNameFromUser(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(x => x.Type == TenantNameClaimType)?.Value;
        }
    }
}

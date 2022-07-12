using AuthPermissions.AdminCode;
using AuthPermissions.AspNetCore;
using AuthPermissions.BaseCode.CommonCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLCubsWebAPI.PermissionsCode;
using PLCubsWebAPI.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PLCubsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantUserController : ControllerBase
    {

        [HasPermission(TenantUserPermission.UserRead)]
        [Route("usertenant")]
        [HttpGet]
        public IActionResult GetLoggedInUserTenant()
        {
            var tenantName = AddTenantNameClaim.GetTenantNameFromUser(User);            
            return Ok(tenantName);
        }

        [HasPermission(TenantUserPermission.UserRead)]
        [Route("GetUserClaim")]
        [HttpGet]
        public IActionResult GetAuthUserInfo([FromServices] IAuthUsersAdminService service)
        {
            var userClaims = User.Claims;
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            string claimsJson = JsonSerializer.Serialize(userClaims, options);
            return Ok(claimsJson);
        }
    }
}

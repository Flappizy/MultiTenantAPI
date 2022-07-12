using AuthPermissions.AdminCode;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using System.ComponentModel.DataAnnotations;

namespace PLCubsWebAPI.Models
{
    public class SingleLevelTenantDto
    {
        public int TenantId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(AuthDbConstants.TenantFullNameSize)]
        public string TenantName { get; set; }

        public string DataKey { get; set; }

        public List<string> TenantRolesName { get; set; }

        public List<string> AllPossibleRoleNames { get; set; }


        public static IQueryable<SingleLevelTenantDto> TurnIntoDisplayFormat(IQueryable<Tenant> inQuery)
        {
            return inQuery.Select(x => new SingleLevelTenantDto
            {
                TenantId = x.TenantId,
                TenantName = x.TenantFullName, 
                DataKey = x.GetTenantDataKey(),
                TenantRolesName = x.TenantRoles.Select(x => x.RoleName).ToList()
            });
        }

        //The IAuthTenantAdminService parameter which contains the AddSingleTenantAsync method that creates a new tenant using the tenant name you provide (and must be unique). Once the new tenant is created its DataKey can be accessed using the tenant’s GetTenantDataKey method, which returns a string with the tenant primary key and a full stop, e.g. “123.”
        public static async Task<SingleLevelTenantDto> SetupForUpdateAsync(IAuthTenantAdminService authTenantAdmin, int tenantId)
        {
            var tenant = (await authTenantAdmin.GetTenantViaIdAsync(tenantId)).Result;
            if (tenant == null)
                throw new AuthPermissionsException($"Could not find the tenant with a TenantId of {tenantId}");

            return new SingleLevelTenantDto
            {
                TenantId = tenantId,
                TenantName = tenant.TenantFullName,
                DataKey = tenant.GetTenantDataKey(),
                TenantRolesName = tenant.TenantRoles.Select(x => x.RoleName).ToList(),
                AllPossibleRoleNames = await authTenantAdmin.GetRoleNamesForTenantsAsync()
            };
        }
    }
}

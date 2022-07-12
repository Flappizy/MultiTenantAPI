using AuthPermissions.SupportCode.AddUsersServices;

namespace PLCubsWebAPI.PermissionsCode
{
    //This specifies the different versions a tenant can have, so there are Gold, Silver and bronze versions of a particular created tenant
    public class CreateTenantVersions
    {
        public static readonly MultiTenantVersionData TenantSetupData = new()
        {
            TenantRolesForEachVersion = new Dictionary<string, List<string>>()
            {
                { "Bronze", null },
                { "Silver", new List<string> { "Tenant Admin" } },
                { "Gold", new List<string> { "Tenant Admin", "SuperAdmin" } },
            },

            TenantAdminRoles = new Dictionary<string, List<string>>()
            {
                { "Bronze", new List<string> { "User admin" } },
                { "Pro", new List<string> { "Tenant Admin", "User admin" } },
                { "Enterprise", new List<string> { "Tenant Admin", "User admin", "SuperAdmin" } }
            }
            //No settings for HasOwnDbForEachVersion as this isn't a sharding 
        };
    }
}

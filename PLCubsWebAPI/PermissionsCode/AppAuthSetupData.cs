using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using AuthPermissions.BaseCode.SetupCode;

namespace PLCubsWebAPI.PermissionsCode
{
    public class AppAuthSetupData
    {
        public static readonly List<BulkLoadRolesDto> RolesDefinition = new List<BulkLoadRolesDto>()
        {
            new("Club History Admin", "Handles club histroy data", "ClubHistoryRead, ClubHistoryModify, UserRead"),
            new("Social Media Admin", "Handles social media post content", "SocialMediaRead, SocialMediaModify, UserRead"),
            new("Fan", "Have access to club history and social media post", "FanReadMediaPost, FanReadClubHistory, UserRead"),
            new("SuperAdmin", "Super admin - mainly for tenant setup", "UserRead, UserSync, UserChange, UserRolesChange, UserChangeTenant, UserRemove, RoleRead, RoleChange, PermissionRead, TenantList, TenantCreate, TenantUpdate, TenantMove, TenantDelete, TenantAccessData, UserRead"),
            new("Tenant Admin", "Tenant-level admin","UserRead, UserRolesChange, RoleRead", RoleTypes.TenantAdminAdd),
            new("User admin", "Manages users in a tenant", "UserRead, UserSync, UserChange, UserRolesChange, UserChangeTenant, UserRemove", RoleTypes.TenantAutoAdd)
        };

        public static readonly List<BulkLoadUserWithRolesTenant> UsersRolesDefinition = new List<BulkLoadUserWithRolesTenant>
        {
            new ("S1admin@arsenal.com", null, "Tenant Admin", tenantNameForDataKey: "Arsenal."),
            new ("S1admin@chelsea.com", null, "Tenant Admin", tenantNameForDataKey: "Chelsea."),
            new ("S1admin@manutd.com", null, "Tenant Admin", tenantNameForDataKey: "Manc Utd."),
            new ("C1lubhistory@arsenal.com", null, "Club History Admin", tenantNameForDataKey: "Arsenal."),
            new ("C1lubhistory@chelsea.com", null, "Club History Admin", tenantNameForDataKey: "Chelsea."),
            new ("C1lubhistory@manutd.com", null, "Club History Admin", tenantNameForDataKey: "Manc Utd."),
            new ("M1ediaadmin@arsenal.com", null, "Social Media Admin", tenantNameForDataKey: "Arsenal."),
            new ("M1ediaadmin@chelsea.com", null, "Social Media Admin", tenantNameForDataKey: "Chelsea."),
            new ("M1ediaadmin@manutd.com", null, "Social Media Admin", tenantNameForDataKey: "Manc Utd."),
            new ("F1an@arsenal.com", null, "Fan", tenantNameForDataKey: "Arsenal."),
            new ("F1an@chelsea.com", null, "Fan", tenantNameForDataKey: "Chelsea."),
            new ("F1an@manutd.com", null, "Fan", tenantNameForDataKey: "Manc Utd."),
        };

        public static readonly List<BulkLoadTenantDto> TenantDefinition = new()
        {
            new("Arsenal.", "Tenant Admin"), 
            new("Chelsea.", "Tenant Admin"),
            new("Manc Utd."),
        };
    }
}

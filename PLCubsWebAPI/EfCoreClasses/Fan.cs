using AuthPermissions.BaseCode.CommonCode;

namespace PLCubsWebAPI.EfCoreClasses
{
    //The IDataKeyFilterReadWrite interface says that you can read and write the DataKey
    public class Fan : IDataKeyFilterReadWrite
    {
        public int FanId { get; set; }

        /// <summary>
        /// This contains the fullname of the AuthP Tenant
        /// </summary>
        public string ClubName { get; set; }

        /// <summary>
        /// This contains the datakey from the AuthP's Tenant
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// This contains the Primary key from the AuthP's Tenant
        /// </summary>
        public int AuthPTenantId { get; set; }
    }
}

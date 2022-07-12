namespace PLCubsWebAPI.Models
{
    public class JwtSetupData
    {
        /// <summary>
        /// This identifies provider that issued the JWT
        /// </summary>
        public string Issuer { get; set; } = null!;
        /// <summary>
        /// This identifies the recipients that the JWT is intended for
        /// </summary>
        public string Audience { get; set; } = null!;
        /// <summary>
        /// This is a SECRET key that both the issuer and audience have to have 
        /// </summary>
        public string SigningKey { get; set; } = null!;
    }
}

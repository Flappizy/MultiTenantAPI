using System.ComponentModel.DataAnnotations;

namespace PLCubsWebAPI.Models
{
    public class LoginUserModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}

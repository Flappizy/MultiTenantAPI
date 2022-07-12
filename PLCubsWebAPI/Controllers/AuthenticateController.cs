using AuthPermissions;
using AuthPermissions.AspNetCore.JwtTokenCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PLCubsWebAPI.Models;

namespace PLCubsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenBuilder _tokenBuilder;

        public AuthenticateController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ITokenBuilder tokenBuilder, IClaimsCalculator claimsCalculator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenBuilder = tokenBuilder;
        }

        /// <summary>
        /// This checks you are a valid user and returns a JTW token
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("authuser")]
        public async Task<ActionResult> Authenticate(LoginUserModel loginUser)
        {
            //NOTE: The _signInManager.PasswordSignInAsync does not change the current ClaimsPrincipal - that only happens on the next access with the token
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, false);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            //Gets the user, from the ASPNETUsers table this is done because the userId will be used to generate a JWT for the user
            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            //Generate and return the JWT for the user to access other aspect of the application
            return Ok(await _tokenBuilder.GenerateJwtTokenAsync(user.Id));
        }
    }
}

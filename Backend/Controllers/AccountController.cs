using Backend.Dtos;
using Backend.Errors;
using Backend.Extensions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService  tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Log In.
        /// </summary>
        /// <param name="loginDto"></param> 
        /// <returns>Return users info: email, name and authentication token.</returns>
        /// <response code="200">Login as user</response>
        /// <response code="401">Such user does not exists</response>
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401, "Such user does not exists."));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        /// <summary>
        /// Sign In.
        /// </summary>
        /// <param name="registerDto"></param> 
        /// <returns>Return users info: email, name and authentication token.</returns>
        /// <response code="200">Register new user</response>
        /// <response code="400">Bad request. Wrong password, empty fields and etc.</response>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is in use."}});
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            // can be error if weak password
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        /// <summary>
        /// Display info for current user depends on token.
        /// </summary>
        /// <returns>Return users info: email, name and authentication token.</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrectUser()
        {
            var user = await _userManager.FindByEmailCustomAsync(HttpContext.User);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }


        /// <summary>
        /// Check if email in use.
        /// </summary>
        /// <param name="email"></param> 
        /// <returns>Return ture/false depends on existing such email in our db</returns>
        /// <response code="200">true or false</response>
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}

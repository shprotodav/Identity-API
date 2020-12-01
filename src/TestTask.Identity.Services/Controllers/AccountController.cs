using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestTask.Identity.BL.DTOs;
using TestTask.Identity.BL.Services;

namespace TestTask.Identity.Services.Controllers
{
    /// <summary>
    /// Main controller.
    /// Contains requests to authorization of users
    /// </summary>
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public AccountController(IUserService userService,
                                IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDTO userForRegisterDto)
        {
            var userToReturn = await _userService.Register(userForRegisterDto);

            return Ok(userToReturn);
        }

        /// <summary>
        /// User login request
        /// </summary>
        /// <remarks>
        /// Use the token from this response to authorization at Transaction API
        /// </remarks>
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserForLoginDTO userForLoginDto)
        {
            var result = await _userService.Login(userForLoginDto);

            return Ok(result);
        }

        /// <summary>
        /// Checks if a token is valid
        /// </summary>
        /// <remarks>
        /// Used to allow execution of requests from Transaction API
        /// </remarks>
        [Route("ValidateToken")]
        [HttpGet]
        public async Task<IActionResult> ValidateToken([FromQuery] string token)
        {
            await _jwtTokenService.ValidateToken(token);

            return Ok();
        }

        [Route("IsAuthenticated")]
        [HttpGet]
        public IActionResult IsAuthenticated()
        {
            return new ObjectResult(new { isAuthenticated = User.Identity.IsAuthenticated });
        }
    }
}

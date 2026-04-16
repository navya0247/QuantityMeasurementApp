using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.DTO;
using AuthService.Interfaces;

namespace AuthService.Controllers
{
    /// <summary>
    /// Handles user authentication.
    /// POST /auth/signup  — Register
    /// POST /auth/signin  — Login, receive JWT
    /// GET  /auth/profile — Get own profile (JWT required)
    /// </summary>
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Register a new user account.</summary>
        [HttpPost("signup")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserProfileDTO), 201)]
        [ProducesResponseType(400)]
        public IActionResult SignUp([FromBody] SignUpDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var profile = _authService.SignUp(dto);
            return StatusCode(201, profile);
        }

        /// <summary>Login and receive a JWT token.</summary>
        [HttpPost("signin")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDTO), 200)]
        [ProducesResponseType(401)]
        public IActionResult SignIn([FromBody] SignInDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = _authService.SignIn(dto);
            return Ok(response);
        }

        /// <summary>Returns the profile of the currently authenticated user.</summary>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? string.Empty;
            var profile = _authService.GetProfile(email);
            return Ok(profile);
        }
    }
}

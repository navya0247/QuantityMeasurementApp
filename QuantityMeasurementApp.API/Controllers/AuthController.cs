using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.API.Controllers
{
    /// <summary>
    /// Handles user authentication.
    /// POST  /auth/signup   — Register
    /// POST  /auth/signin   — Login, receive JWT
    /// POST  /auth/google   — Google OAuth sign-in / auto-register
    /// GET   /auth/profile  — Get own profile (JWT required)
    /// </summary>
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService        _authService;
        private readonly IGoogleAuthService  _googleAuthService;

        public AuthController(IAuthService authService, IGoogleAuthService googleAuthService)
        {
            _authService       = authService;
            _googleAuthService = googleAuthService;
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

        /// <summary>
        /// Sign in (or auto-register) using a Google ID token.
        /// The frontend sends the raw credential from Google Identity Services.
        /// </summary>
        [HttpPost("google")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleAuthDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.IdToken))
                return BadRequest(new { message = "Google ID token is required." });

            try
            {
                var response = await _googleAuthService.SignInWithGoogleAsync(dto.IdToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>Returns the profile of the currently authenticated user.</summary>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email)
                        ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = _authService.GetProfile(email);
            return Ok(profile);
        }
    }
}

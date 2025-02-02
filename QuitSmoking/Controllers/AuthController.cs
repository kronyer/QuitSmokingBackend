using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuitSmoking.Application.DTOs;
using QuitSmoking.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuitSmoking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager; // Add this line

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager) // Modify constructor
        {
            _authService = authService;
            _userManager = userManager; // Initialize _userManager
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model);

            if (result == "User created successfully")
            {
                return Ok(new { Result = result });
            }

            return BadRequest(new { Error = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(model);

            if (result.Success)
            {
                return Ok(result);
            }

            return Unauthorized(new { Error = "Error" });
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] TokenRequestDto tokenRequest)
        {
            var isValid = await _authService.ValidateTokenAsync(tokenRequest.Token);
            if (isValid)
            {
                return Ok(new { Valid = true });
            }
            return Unauthorized(new { Valid = false });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var refreshToken = Request.Headers["X-Refresh-Token"].ToString();

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { Error = "Tokens não foram fornecidos." });
            }

            var tokenRequest = new TokenRequestDto
            {
                Token = accessToken,
                RefreshToken = refreshToken
            };

            var newTokens = await _authService.RefreshTokenAsync(tokenRequest);
            if (newTokens == null)
            {
                return Unauthorized(new { Error = "Refresh Token inválido ou expirado." });
            }

            return Ok(new
            {
                newTokens.AccessToken,
                newTokens.RefreshToken
            });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] TokenRequestDto tokenRequest)
        {
            await _authService.LogoutAsync(tokenRequest.RefreshToken);
            return Ok(new { Result = "Logged out successfully." });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.ChangePasswordAsync(userId, model);
            if (result == "Password changed successfully.")
            {
                return Ok(new { Result = result });
            }
            return BadRequest(new { Error = result });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var result = await _authService.ForgotPasswordAsync(model);
            if (result == "Password reset email sent.")
            {
                return Ok(new { Result = result });
            }
            return BadRequest(new { Error = result });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _authService.ResetPasswordAsync(model);
            if (result == "Password reset successfully.")
            {
                return Ok(new { Result = result });
            }
            return BadRequest(new { Error = result });
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto model)
        {
            var result = await _authService.ConfirmEmailAsync(model);
            if (result == "Email confirmed successfully.")
            {
                return Ok(new { Result = result });
            }
            return BadRequest(new { Error = result });
        }

        [HttpGet("session")]
        public async Task<IActionResult> Session()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isActive = await _authService.IsSessionActiveAsync(userId);
            return Ok(new { Active = isActive });
        }

        [HttpGet("is-email-taken")]
        public async Task<IActionResult> IsEmailTaken([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Error = "Email não fornecido." });
            }

            var isTaken = await _authService.IsEmailTakenAsync(email);
            return Ok(new { IsEmailTaken = isTaken });
        }



        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto model)
        {
            var payload = await _authService.VerifyGoogleToken(model);
            if (payload == null)
            {
                return Unauthorized(new { Error = "Token do Google inválido." });
            }

            var user = await _authService.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            var token = await _authService.GenerateJwtToken(user);
            return Ok(new LoginResponseDto
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                Message = "Login bem-sucedido.",
                Success = true
            });
        }


    }
}

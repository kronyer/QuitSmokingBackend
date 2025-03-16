using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuitSmoking.Application.DTOs;
using QuitSmoking.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2; // Adicione esta linha

namespace QuitSmoking.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ISmokingProgressServices _smokingProgressService;
        private readonly GoogleAuthorizationCodeFlow _googleAuthorizationCodeFlow; // Adicione esta linha


        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ISmokingProgressServices smokingProgressServices )
        {
            _userManager = userManager;
            _smokingProgressService = smokingProgressServices;
            _signInManager = signInManager;
            _configuration = configuration;
            _googleAuthorizationCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer // Adicione esta linha
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _configuration["Authentication:Google:ClientId"],
                    ClientSecret = _configuration["Authentication:Google:ClientSecret"]
                }
            }); // Adicione esta linha
        }

        public async Task<string> RegisterAsync(RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return "Passwords do not match.";
            }

            var user = new ApplicationUser {
                UserName = model.Email, Email = model.Email, NormalizedUserName = model.Name ,
                BirthDate = model.BirthDate,
                PhoneNumber = model.Phone


            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return "User created successfully";
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return errors;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new LoginResponseDto {Token="", RefreshToken="", Message= "User not found.", Success = false };
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Definir a data de expiração do token de atualização
                await _userManager.UpdateAsync(user);
                var token =  await GenerateJwtToken(user);

                await _smokingProgressService.IsSuccess(user.Id);

                return new LoginResponseDto { Token = token, RefreshToken = user.RefreshToken, Message = "User logged-in.", Success = true };

            }

            if (result.IsLockedOut)
            {
                return new LoginResponseDto { Token = "", RefreshToken = "", Message = "User account locked out.", Success = false };

            }

            if (result.IsNotAllowed)
            {
                return new LoginResponseDto { Token = "", RefreshToken = "", Message = "User not allowed to sign in.", Success = false };

            }

            //pm retorna o LoginResponseDto

            return new LoginResponseDto { Token = "", RefreshToken = "", Message = "Invalid password.", Success = false };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(TokenRequestDto tokenRequest)
        {
            var principal = GetPrincipalFromExpiredToken(tokenRequest.Token);
            var user = await _userManager.FindByEmailAsync(principal.Identity.Name);

            if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }

            var newToken = await GenerateJwtToken(user);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Atualize a data de expiração do token de atualização
            await _userManager.UpdateAsync(user);

            return new TokenResponseDto
            {
                AccessToken = newToken,
                RefreshToken = user.RefreshToken
            };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                return;
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task<string> ChangePasswordAsync(string userId, ChangePasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found.";
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return "Password changed successfully.";
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return errors;
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return "User not found.";
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Enviar e-mail com o token de redefinição de senha
            return "Password reset email sent.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return "User not found.";
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return "Password reset successfully.";
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return errors;
        }

        public async Task<string> ConfirmEmailAsync(ConfirmEmailDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return "User not found.";
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
            if (result.Succeeded)
            {
                return "Email confirmed successfully.";
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return errors;
        }

        public async Task<bool> IsSessionActiveAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Configurar o sub claim com o ID do usuário
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<LoginResponseDto> LoginWithGoogle(GoogleLoginDto model)
        {
            var payload = await VerifyGoogleCode(model);
            if (payload == null)
            {
                return new LoginResponseDto { Token = "", RefreshToken = "", Message = "Invalid Google code.", Success = false };
            }
            var user = await FindByEmailAsync(payload.Email);
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
                    return new LoginResponseDto { Token = "", RefreshToken = "", Message = "Error creating user.", Success = false };
                }
            }
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Set refresh token expiry time
            await _userManager.UpdateAsync(user);
            var token = await GenerateJwtToken(user);
            return new LoginResponseDto { Token = token, RefreshToken = user.RefreshToken, Message = "User logged-in.", Success = true };
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleCode(GoogleLoginDto model)
        {
            var code = model.Code;
            var clientId = _configuration["Authentication:Google:ClientId"];
            var clientSecret = _configuration["Authentication:Google:ClientSecret"];
            var redirectUri = _configuration["Authentication:Google:RedirectUri"];

            var tokenResponse = await _googleAuthorizationCodeFlow.ExchangeCodeForTokenAsync("user-id", code, redirectUri, CancellationToken.None);
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { clientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken, settings);
            return payload;
        }

        public async Task<bool> isFirstAccess(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return user.CigarreteId == null || user.CigarreteId == 0;
        }
    }
}

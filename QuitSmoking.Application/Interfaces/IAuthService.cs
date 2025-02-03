using Google.Apis.Auth;
using QuitSmoking.Application.DTOs;
using QuitSmoking.Domain.Entities;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterModel model);
    Task<LoginResponseDto> LoginAsync(LoginModel model);
    Task<bool> ValidateTokenAsync(string token);
    Task<TokenResponseDto> RefreshTokenAsync(TokenRequestDto tokenRequest); // Change return type to TokenResponseDto
    Task LogoutAsync(string refreshToken);
    Task<string> ChangePasswordAsync(string userId, ChangePasswordDto model);
    Task<string> ForgotPasswordAsync(ForgotPasswordDto model);
    Task<string> ResetPasswordAsync(ResetPasswordDto model);
    Task<string> ConfirmEmailAsync(ConfirmEmailDto model);
    Task<bool> IsSessionActiveAsync(string userId);
    Task<ApplicationUser> FindByEmailAsync(string email);
    Task<string> GenerateJwtToken(ApplicationUser user);
    Task<bool> IsEmailTakenAsync(string email);
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleCode(GoogleLoginDto googleLogin); // Added missing method signature
    Task<LoginResponseDto> LoginWithGoogle(GoogleLoginDto model);
}

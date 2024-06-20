using userauthentication.DTO.Request;
using userauthentication.DTO.Response;

namespace userauthentication.Services
{
	public interface IAuthService
	{
		Task<GeneralResponse> Register(UserRegisterRequest request);
		Task<LoginResponse?> Login(UserLoginRequest request);
		Task<GeneralResponse> Verify(string varificatonToken);
		Task<GeneralResponse> RegenerateVerifyToken(string email);
		Task<GeneralResponse> ForgotPassword(ForgotPasswordRequest request);
		Task<GeneralResponse> ResetPassword(PasswordResetRequest request);
		Task<LoginResponse?> RefreshToken(string RefreshToken);
		Task Logout();
	}
}
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;

namespace userauthentication.Repository;

public interface IAuthRepository
{
	Task<GeneralResponse> Register(UserRegisterRequest request);

	Task<TokenResponse?> Login(UserLoginRequest request);

	Task<GeneralResponse> Verify(string varificatonToken);

	Task<GeneralResponse> RegenerateVerifyToken(string email);

	Task<GeneralResponse> ForgotPassword(ForgotPasswordRequest request);

	Task<GeneralResponse> ResetPassword(PasswordResetRequest request);
}
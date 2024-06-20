using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using userauthentication.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using userauthentication.Repositories;

namespace userauthentication.Services.Implementation
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;
		private readonly IUserRepository _userrepository;
		private readonly IService _service;

		private const string UserDefaultPrivilege = "User";
		private const int ResetTokenExpirationTimeInHours = 4;
		private const double JWTExpirationTimeInMinutes = .08333; // 5 Minutes
		private const int RefreshTokenExpirationTimeInHours = 4;

		public AuthService(IConfiguration configuration, IUserRepository userrepository, IService service)
		{
			_configuration = configuration;
			_userrepository = userrepository;
			_service = service;
		}

		// User Register
		public async Task<GeneralResponse> Register(UserRegisterRequest request)
		{
			if (_userrepository.FindByEmail(request.Email) != null)
				return new GeneralResponse(false, "Email Already Exist.");

			await _service.RegisterUserAsync(
				request.Email,
				request.Password,
				UserDefaultPrivilege    // Default Privilege of an User
			);

			return new GeneralResponse(true, "User Registered Successfully.");
		}

		// Login User
		public async Task<LoginResponse?> Login(UserLoginRequest request)
		{
			User? user = _userrepository.FindByEmail(request.Email);

			if (user == null) return null;

			if (user.VerifiedAt == null) return null;

			if (!VerifyPasswordHash.VerifyHash(request.Password,
				user.PasswordHash, user.PasswordSalt))
			{
				return null;
			}

			string authToken = CreateAccessToken(user);
			string refreshToken = CreateRefreshToken.RefreshToken();

			// Check For Duplicate Entry
			while (_userrepository.FindByRefreshToken(refreshToken) != null)
			{
				refreshToken = CreateRefreshToken.RefreshToken();
			}

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpirationTime = DateTime.Now.AddHours(RefreshTokenExpirationTimeInHours);

			await _userrepository.SaveAsync();

			return new LoginResponse(true, authToken, refreshToken, "Login Successful.");
		}

		// Refresh Token
		public async Task<LoginResponse?> RefreshToken(string RefreshToken)
		{
			User? user = _userrepository.FindByRefreshToken(RefreshToken);
			if (user == null) return null;

			if (user.RefreshTokenExpirationTime < DateTime.Now) return null;

			string authToken = CreateAccessToken(user);

			return new LoginResponse(
				true,
				authToken,
				RefreshToken,
				"New Access Token Generaed Successfully."
			);
		}

		// Verify User
		public async Task<GeneralResponse> Verify(string varificatonToken)
		{
			User? user = _userrepository.FindByVerificationToken(varificatonToken);

			if (user == null) return new GeneralResponse(false, "Invalid Token.");

			user.VerifiedAt = DateTime.Now;

			await _userrepository.SaveAsync();

			return new GeneralResponse(true, "Verified Successfully.");
		}

		// Regenerate Verify Token
		public async Task<GeneralResponse> RegenerateVerifyToken(string email)
		{
			User? user = _userrepository.FindByEmail(email);

			if (user == null)
				return new GeneralResponse(false, "Unable To generate Verification Token");

			if (user.VerifiedAt != null)
				return new GeneralResponse(false, "Unable To generate Verification Token");

			user.EmailVerificationToken = CreateVerifyToken.CreateRandomToken();

			await _userrepository.SaveAsync();

			return new GeneralResponse(true, "Verificaton Token Generaed Successfully.");
		}

		// Forgot Password
		public async Task<GeneralResponse> ForgotPassword(ForgotPasswordRequest request)
		{
			User? user = _userrepository.FindByEmail(request.Email);

			if (user == null) return new GeneralResponse(false, "Unable to process request.");

			user.PasswordResetToken = CreateVerifyToken.CreateRandomToken();
			user.PasswordResetTokenExpires = DateTime.Now.AddHours(ResetTokenExpirationTimeInHours);

			await _userrepository.SaveAsync();

			return new GeneralResponse(true, "Password Reset Token Generaed Successfully.");
		}

		// Reset Password
		public async Task<GeneralResponse> ResetPassword(PasswordResetRequest request)
		{
			var user = _userrepository.FindByPasswordResetToken(request.Token);

			if (user == null || user.PasswordResetTokenExpires < DateTime.Now)
			{
				return new GeneralResponse(false, "Invalid User or Token");
			}

			CreatePasswordHash.CreateHash(request.Password, out byte[] passwordhash, out byte[] passwordsalt);

			user.PasswordHash = passwordhash;
			user.PasswordSalt = passwordsalt;
			user.PasswordResetToken = null;
			user.PasswordResetTokenExpires = null;
			user.RefreshToken = null;
			user.RefreshTokenExpirationTime = null;

			await _userrepository.SaveAsync();

			return new GeneralResponse(true, "Password Reset Successfully.");
		}

		// Logout User
		public async Task Logout()
		{
			string userInfo = _service.getCurrentUser(ClaimTypes.Name);

			User? user = _userrepository.FindById(userInfo);

			user.RefreshToken = null;
			user.RefreshTokenExpirationTime = null;

			await _userrepository.SaveAsync();
		}

		// Helper Methods
		private string CreateAccessToken(User user)
		{
			string secretKey = _configuration.GetSection("AppSettings:JWTSecretKey").Value!;
			string authToken = CreateJWTToken.CreateToken(
				user,
				secretKey,
				JWTExpirationTimeInMinutes
			);
			return authToken;
		}
	}
}
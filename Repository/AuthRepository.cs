using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using userauthentication.Data;
using userauthentication.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using userauthentication.Services;

namespace userauthentication.Repository;

public class AuthRepository : IAuthRepository
{
	private readonly UserDbContext _context;
	private readonly IConfiguration _configuration;
	private readonly IUserService _userservice;

	private const string UserDefaultPrivilege = "User";
	private const int ResetTokenExpirationTimeInHours = 4;
	private const int JWTExpirationTimeInHours = 4;

	public AuthRepository( UserDbContext context, IConfiguration configuration, 
		IUserService userservice )
	{
		_context = context;
		_configuration = configuration;
		_userservice = userservice;
	}

	// User Register
	public async Task<GeneralResponse> Register(UserRegisterRequest request)
	{
		if(_context.Users.Any(u => u.Email == request.Email))
		return new GeneralResponse(false, "Email Already Exist.");

		await _userservice.RegisterUserAsync(
			request.Email, 
			request.Password, 
			UserDefaultPrivilege    // Default Privilege of an User
		);

		return new GeneralResponse(true, "User Registered Successfully.");
	}

	// Login User
	public async Task<TokenResponse?> Login(UserLoginRequest request)
	{
		User user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

		if(user == null) return null;

		if(user.VerifiedAt == null) return null;

		if( !VerifyPasswordHash.VerifyHash(request.Password, 
			user.PasswordHash, user.PasswordSalt) )
		{
			return null;
		}

		string secretKey = _configuration.GetSection("AppSettings:JWTSecretKey").Value!;
		string authToken = CreateJWTToken.CreateToken(
			user, 
			secretKey, 
			JWTExpirationTimeInHours
		);

		return new TokenResponse(true, authToken, "Login Successful.");
	}

	// Verify User
	public async Task<GeneralResponse> Verify(string varificatonToken)
	{
		var user = _context.Users.FirstOrDefault(u => u.EmailVerificationToken == varificatonToken);

		if(user == null) return new GeneralResponse(false, "Invalid Token.");

		user.VerifiedAt = DateTime.Now;

		await _context.SaveChangesAsync();

		return new GeneralResponse(true, "Verified Successfully.");
	}

	// Regenerate Verify Token
	public async Task<GeneralResponse> RegenerateVerifyToken(string email)
	{
		var user = _context.Users.FirstOrDefault(u => u.Email == email);

		if(user == null) 
		return new GeneralResponse(false, "Unable To generate Verification Token");

		if(user.VerifiedAt != null) 
		return new GeneralResponse(false, "Unable To generate Verification Token");

		user.EmailVerificationToken = CreateVerifyToken.CreateRandomToken();

		await _context.SaveChangesAsync();

		return new GeneralResponse(true, "Verificaton Token Generaed Successfully.");
	}

	// Forgot Password
	public async Task<GeneralResponse> ForgotPassword(ForgotPasswordRequest request)
	{
		var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

		if(user == null) return new GeneralResponse(false, "Unable to process request.");

		user.PasswordResetToken = CreateVerifyToken.CreateRandomToken();
		user.PasswordResetTokenExpires = DateTime.Now.AddHours(ResetTokenExpirationTimeInHours);

		await _context.SaveChangesAsync();

		return new GeneralResponse(true, "Password Reset Token Generaed Successfully.");
	}

	// Reset Password
	public async Task<GeneralResponse> ResetPassword(PasswordResetRequest request)
	{
		var user = _context.Users.FirstOrDefault(u => u.PasswordResetToken == request.Token);

		if(user == null || user.PasswordResetTokenExpires < DateTime.Now)
		{
			return new GeneralResponse(false, "Invalid User or Token");
		}

		CreatePasswordHash.CreateHash(request.Password, out byte[] passwordhash, out byte[] passwordsalt);

		user.PasswordHash = passwordhash;
		user.PasswordSalt = passwordsalt;
		user.PasswordResetToken = null;
		user.PasswordResetTokenExpires = null;

		await _context.SaveChangesAsync();

		return new GeneralResponse(true, "Password Reset Successfully.");
	}
}
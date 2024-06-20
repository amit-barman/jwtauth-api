using Microsoft.AspNetCore.Mvc;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using userauthentication.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authservice;
	private readonly IUserService _userservice;

	public AuthController(IAuthService authservice, IUserService userservice)
	{
		_authservice = authservice;
		_userservice = userservice;
	}

	[HttpPost("register")]
	public async Task<ActionResult> Register(UserRegisterRequest request)
	{
		GeneralResponse result = await _authservice.Register(request);
		if (result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPost("login")]
	public async Task<ActionResult> Login(UserLoginRequest request)
	{
		LoginResponse? result = await _authservice.Login(request);

		if (result == null) return BadRequest(
			new GeneralResponse(false, "Invalid Login Credentials.")
		);

		return Ok(result);
	}

	[HttpPost("refresh-token")]
	public async Task<ActionResult> RefreshToken([FromBody] string RefreshToken)
	{
		LoginResponse? result = await _authservice.RefreshToken(RefreshToken);

		if (result == null) return Unauthorized(
			new GeneralResponse(false, "Invalid Refresh Token.")
		);

		return Ok(result);
	}

	[HttpGet("verify/{verificationToken}")]
	public async Task<ActionResult> Verify(string verificationToken)
	{
		GeneralResponse result = await _authservice.Verify(verificationToken);
		if (result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPut("regenerate-verification-token/{email}")]
	public async Task<ActionResult> RegenerateVerifyToken(string email)
	{
		GeneralResponse result = await _authservice.RegenerateVerifyToken(email);
		if (result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPost("forgot-password")]
	public async Task<ActionResult> ForgotPassword(ForgotPasswordRequest request)
	{
		GeneralResponse result = await _authservice.ForgotPassword(request);
		if (result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPost("reset-password")]
	public async Task<ActionResult> ResetPassword(PasswordResetRequest request)
	{
		GeneralResponse result = await _authservice.ResetPassword(request);
		if (result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpDelete("logout"), Authorize]
	public async Task<ActionResult> Logout()
	{
		await _authservice.Logout();
		return Ok(new GeneralResponse(true, "User Logout Successfully."));
	}

	// User After Authorization
	[HttpGet("user-info"), Authorize]
	public ActionResult<UserInfoResponse> UserInfo()
	{
		return Ok(_userservice.UserInfo());
	}

	[HttpPost("update-user"), Authorize]
	public async Task<ActionResult> UpdateUserInfo(UserUpdateRequest request)
	{
		GeneralResponse result = await _userservice.UpdateUserInfo(request);
		return Ok(result);
	}
}
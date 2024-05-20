using Microsoft.AspNetCore.Mvc;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using userauthentication.Repository;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthRepository _authrepository;
	private readonly IUserRepository _usersrepository;

	public AuthController(IAuthRepository authrepository, IUserRepository usersrepository)
	{
		_authrepository = authrepository;
		_usersrepository = usersrepository;
	}

	[HttpPost("register")]
	public async Task<ActionResult> Register(UserRegisterRequest request)
	{
		GeneralResponse result = await _authrepository.Register(request);
		if(result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPost("login")]
	public async Task<ActionResult> Login(UserLoginRequest request)
	{
		TokenResponse? result = await _authrepository.Login(request);

		if(result == null) return BadRequest(
			new GeneralResponse(false, "Invalid Login Credentials.")
		);

		return Ok(result);
	}

	[HttpGet("verify/{verificationToken}")]
	public async Task<ActionResult> Verify(string verificationToken)
	{
		GeneralResponse result = await _authrepository.Verify(verificationToken);
		if(result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPut("regenerate-verification-token/{email}")]
	public async Task<ActionResult> RegenerateVerifyToken(string email)
	{
		GeneralResponse result = await _authrepository.RegenerateVerifyToken(email);
		if(result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPost("forgot-password")]
	public async Task<ActionResult> ForgotPassword(ForgotPasswordRequest request)
	{
		GeneralResponse result = await _authrepository.ForgotPassword(request);
		if(result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPost("reset-password")]
	public async Task<ActionResult> ResetPassword(PasswordResetRequest request)
	{
		GeneralResponse result = await _authrepository.ResetPassword(request);
		if(result.status == false) return BadRequest(result);
		return Ok(result);
	}

	// User After Authorization
	[HttpGet("user-info"), Authorize]
	public ActionResult<UserInfoResponse> UserInfo()
	{
		return Ok(_usersrepository.UserInfo());
	}

	[HttpPost("update-user"), Authorize]
	public async Task<ActionResult> UpdateUserInfo(UserUpdateRequest request)
	{
		GeneralResponse result = await _usersrepository.UpdateUserInfo(request);
		return Ok(result);
	}
}
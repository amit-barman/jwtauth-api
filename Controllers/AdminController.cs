using Microsoft.AspNetCore.Mvc;
using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using userauthentication.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
	private readonly IAdminService _adminservice;

	public AdminController(IAdminService adminservice)
	{
		_adminservice = adminservice;
	}

	[HttpGet("users")]
	public async Task<ActionResult<List<User>>> Users()
	{
		return Ok(_adminservice.Users());
	}

	[HttpGet("search-user/{Uid}")]
	public async Task<ActionResult> SearchUser(string Uid)
	{
		var user = await _adminservice.SearchUser(Uid);

		if (user == null)
		{
			return BadRequest(new GeneralResponse(false, "User Not Found"));
		}

		return Ok(user);
	}

	[HttpPost("register-user")]
	public async Task<ActionResult> Register(UserRegisterRequestAdmin request)
	{
		GeneralResponse result = await _adminservice.Register(request);
		if (result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpDelete("remove-user/{Uid}")]
	public async Task<IActionResult> RemoveUser(string Uid)
	{
		GeneralResponse result = await _adminservice.RemoveUser(Uid);
		if (result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpPost("edit-user")]
	public async Task<ActionResult> EditUser(UserEditRequest request)
	{
		GeneralResponse result = await _adminservice.UpdateUser(request);
		if (result.status == false) return BadRequest(result);
		return Ok(result);
	}
}
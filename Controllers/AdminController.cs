using Microsoft.AspNetCore.Mvc;
using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using userauthentication.Repositories;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
	private readonly IAdminRepository _adminrepository;

	public AdminController(IAdminRepository adminrepository)
	{
		_adminrepository = adminrepository;
	}

	[HttpGet("users")]
	public async Task<ActionResult<List<User>>> Users()
	{
		return Ok( _adminrepository.Users() );
	}

	[HttpGet("search-user/{Uid}")]
	public async Task<ActionResult> SearchUser(string Uid)
	{
		var user = await _adminrepository.SearchUser(Uid);

		if(user == null)
		{
			return BadRequest( new GeneralResponse(false, "User Not Found") );
		}

		return Ok(user);
	}

	[HttpPost("register-user")]
	public async Task<ActionResult> Register(UserRegisterRequestAdmin request)
	{
		GeneralResponse result = await _adminrepository.Register(request);
		if(result.status == false) return BadRequest(result);
		return Ok(result);
	}

	[HttpDelete("remove-user/{Uid}")]
	public async Task<IActionResult> RemoveUser(string Uid)
	{
	    GeneralResponse result = await _adminrepository.RemoveUser(Uid);
	    if(result.status == false) return BadRequest(result);
	    return Ok(result);
	}

	[HttpPost("edit-user")]
	public async Task<ActionResult> EditUser(UserEditRequest request)
	{
		GeneralResponse result = await _adminrepository.UpdateUser(request);
	    if(result.status == false) return BadRequest(result);
	    return Ok(result);
	}
}
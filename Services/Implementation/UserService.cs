using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using userauthentication.Data;
using userauthentication.Utilities;
using System.Security.Claims;
using userauthentication.Services;
using userauthentication.Repositories;

namespace userauthentication.Services.Implementation;

public class UserService : IUserService
{
	private readonly IUserRepository _userrepository;
	private readonly IService _service;

	public UserService(IUserRepository userrepository, IService service)
	{
		_userrepository = userrepository;
		_service = service;
	}

	// User Info
	public UserInfoResponse UserInfo()
	{		
		string userInfo = _service.getCurrentUser(ClaimTypes.Name);

		User? user = _userrepository.FindById(userInfo);

		return new UserInfoResponse(
			user.Id,
			user.Uid,
			user.Email,
			user.CreatedAt,
			user.AccountType,
			user.VerifiedAt
		);
	}

	public async Task<GeneralResponse> UpdateUserInfo(UserUpdateRequest request)
	{
		string userInfo = _service.getCurrentUser(ClaimTypes.Name);

		// Updated Data
		bool status = await _service.UpdateUserDataAsync(userInfo, request.Email, null);

		if(!status) return new GeneralResponse(false, "Unable to Update Information");

		return new GeneralResponse(true, "Information Updated Successfully.");
	}
}
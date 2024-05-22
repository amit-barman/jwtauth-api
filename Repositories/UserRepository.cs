using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using userauthentication.Data;
using userauthentication.Utilities;
using System.Security.Claims;
using userauthentication.Services;

namespace userauthentication.Repositories;

public class UserRepository : IUserRepository
{
	private readonly UserDbContext _context;
	private readonly IUserService _userservice;

	public UserRepository(UserDbContext context, IUserService userservice)
	{
		_context = context;
		_userservice = userservice;
	}

	// User Info
	public UserInfoResponse UserInfo()
	{		
		string userInfo = _userservice.getCurrentUser(ClaimTypes.Name);

		User user = _context.Users.Single(user => user.Uid == userInfo);

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
		string userInfo = _userservice.getCurrentUser(ClaimTypes.Name);

		// Updated Data
		bool status = await _userservice.UpdateUserDataAsync(userInfo, request.Email, null);

		if(!status) return new GeneralResponse(false, "Unable to Update Information");

		return new GeneralResponse(true, "Information Updated Successfully.");
	}
}
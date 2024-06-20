using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using userauthentication.Data;
using userauthentication.Utilities;
using System.Security.Claims;
using userauthentication.Repositories;

namespace userauthentication.Services.Implementation;

public class AdminService : IAdminService
{
	private readonly IUserRepository _userrepository;
	private readonly IService _service;

	public AdminService(IUserRepository userrepository, IService service)
	{
		_userrepository = userrepository;
		_service = service;
	}

	// Show All Users
	public async Task<List<User>> Users()
	{
		return _userrepository.GetAll();
	}

	// Search User by UID
	public async Task<User?> SearchUser(string Uid)
	{
		User? user = _userrepository.FindById(Uid);
		if (user == null) return null;
		return user;
	}

	// Register New User With Privileges
	public async Task<GeneralResponse> Register(UserRegisterRequestAdmin request)
	{
		if (_userrepository.FindByEmail(request.Email) != null)
			return new GeneralResponse(true, "Email Already Exist.");

		await _service.RegisterUserAsync(
			request.Email,
			request.Password,
			request.AccountType
		);

		return new GeneralResponse(true, "User Registered Successfully.");
	}

	// Delete User
	public async Task<GeneralResponse> RemoveUser(string Uid)
	{
		var user = _userrepository.FindById(Uid);

		string currentUser = _service.getCurrentUser(ClaimTypes.Name);

		if (user == null) return new GeneralResponse(false, "User Not Found!");

		if (user.Uid == currentUser)
			return new GeneralResponse(false, "You Can Not Remove Your Self!");

		_userrepository.Remove(user);
		await _userrepository.SaveAsync();
		return new GeneralResponse(true, "User Deleted Successfully.");
	}

	// Update Any User Info using UID
	public async Task<GeneralResponse> UpdateUser(UserEditRequest request)
	{
		bool status = await _service.UpdateUserDataAsync(
			request.Uid,
			request.Email,
			request.AccountType
		);

		if (!status)
		{
			return new GeneralResponse(false, "Unable to Update Information!");
		}

		return new GeneralResponse(true, "Information Updated Successfully.");
	}
}
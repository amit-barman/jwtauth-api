using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using userauthentication.Data;
using userauthentication.Utilities;
using System.Security.Claims;
using userauthentication.Services;

namespace userauthentication.Repositories;

public class AdminRepository : IAdminRepository
{
	private readonly UserDbContext _context;
	private readonly IUserService _userservice;

	public AdminRepository(UserDbContext context, IUserService userservice)
	{
		_context = context;
		_userservice = userservice;
	}

	// Show All Users
	public async Task<List<User>> Users()
	{
		return _context.Users.ToList();
	}

	// Search User by UID
	public async Task<User?> SearchUser(string Uid)
	{
		var user = _context.Users.FirstOrDefault(u => u.Uid == Uid);
		if(user == null) return null;
		return user;
	}

	// Register New User With Privileges
	public async Task<GeneralResponse> Register(UserRegisterRequestAdmin request)
	{
		if(_context.Users.Any(u => u.Email == request.Email)) 
		return new GeneralResponse(true, "Email Already Exist.");

		await _userservice.RegisterUserAsync(
			request.Email, 
			request.Password, 
			request.AccountType
		);

		return new GeneralResponse(true, "User Registered Successfully.");
	}

	// Delete User
	public async Task<GeneralResponse> RemoveUser(string Uid)
	{
	    var user = _context.Users.FirstOrDefault(u => u.Uid == Uid);

	    string currentUser = _userservice.getCurrentUser(ClaimTypes.Name);

	    if (user == null) return new GeneralResponse(false, "User Not Found!");

	    if(user.Uid == currentUser) 
	    return new GeneralResponse(false, "You Can Not Remove Your Self!");

	    _context.Users.Remove(user);
	    await _context.SaveChangesAsync();
	    return new GeneralResponse(true, "User Deleted Successfully.");
	}

	// Update Any User Info using UID
	public async Task<GeneralResponse> UpdateUser(UserEditRequest request)
	{
		bool status = await _userservice.UpdateUserDataAsync(
			request.Uid, 
			request.Email, 
			request.AccountType
		);

		if(!status)
		{
			return new GeneralResponse(false, "Unable to Update Information!");
		}

		return new GeneralResponse(true, "Information Updated Successfully.");
	}
}
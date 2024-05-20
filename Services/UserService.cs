using userauthentication.Models;
using userauthentication.Data;
using userauthentication.Utilities;
using System.Security.Claims;

namespace userauthentication.Services;

public class UserService : IUserService
{
	private readonly UserDbContext _context;
	private readonly IHttpContextAccessor _httpconext;

	public UserService(UserDbContext context, IHttpContextAccessor httpconext)
	{
		_context = context;
		_httpconext = httpconext;
	}

	// Register User
	public async Task RegisterUserAsync(string email, string password, string role)
	{
		string uid = Guid.NewGuid().ToString();

		while(_context.Users.Any(u => u.Uid == uid))
		{
			uid = Guid.NewGuid().ToString();
		}

		CreatePasswordHash.CreateHash(password, out byte[] passwdHash, out byte[] passSalt);

		string emailVerificationToken = CreateVerifyToken.CreateRandomToken();

		var user = new User
		{
			Uid = uid,
			Email = email,
			PasswordHash = passwdHash,
			PasswordSalt = passSalt,
			CreatedAt = DateTime.Now,
			AccountType = role,
			EmailVerificationToken = emailVerificationToken
		};

		_context.Users.Add(user);
		await _context.SaveChangesAsync();
	}

	// Update User Data
	public async Task<bool> UpdateUserDataAsync(string Uid, string? Email, 
		string? AccountType)
	{
		User user = _context.Users.FirstOrDefault(u => u.Uid == Uid);

		if(user == null) return false;
		
		if(_context.Users.Any(u => u.Email == Email) && user.Email != Email) return false;

		user.Email = Email ?? user.Email;
		user.AccountType = AccountType ?? user.AccountType;

		await _context.SaveChangesAsync();

		return true;
	}

	// Get Claimed User
	public string getCurrentUser(string ClaimedUser)
	{
		return _httpconext.HttpContext.User.FindFirstValue(ClaimedUser);
	}
}
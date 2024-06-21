using userauthentication.Models;
using userauthentication.Data;
using userauthentication.Utilities;
using System.Security.Claims;
using userauthentication.Repositories;

namespace userauthentication.Services.Implementation;

public class Service : IService
{
	private readonly IUserRepository _userrepository;
	private readonly IHttpContextAccessor _httpconext;

	public Service(IUserRepository userrepository, IHttpContextAccessor httpconext)
	{
		_userrepository = userrepository;
		_httpconext = httpconext;
	}

	// Register User
	public async Task RegisterUserAsync(string email, string password, string role)
	{
		string uid = Guid.NewGuid().ToString();

		while (_userrepository.FindById(uid) != null)
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

		_userrepository.Add(user);
		await _userrepository.SaveAsync();
	}

	// Get Claimed User
	public string getCurrentUser(string ClaimedUser)
	{
		return _httpconext.HttpContext.User.FindFirstValue(ClaimedUser);
	}
}
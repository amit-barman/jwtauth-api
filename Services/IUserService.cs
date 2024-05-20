using userauthentication.Models;

namespace userauthentication.Services;

public interface IUserService
{
	Task RegisterUserAsync(string email, string password, string role);

	Task<bool> UpdateUserDataAsync(string Uid, string? Email, string? AccountType);

	string getCurrentUser(string ClaimedUser);
}
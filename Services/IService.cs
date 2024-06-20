namespace userauthentication.Services;

public interface IService
{
	Task RegisterUserAsync(string email, string password, string role);

	Task<bool> UpdateUserDataAsync(string Uid, string? Email, string? AccountType);

	string getCurrentUser(string ClaimedUser);
}
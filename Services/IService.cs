namespace userauthentication.Services;

public interface IService
{
	Task RegisterUserAsync(string email, string password, string role);
	string getCurrentUser(string ClaimedUser);
}
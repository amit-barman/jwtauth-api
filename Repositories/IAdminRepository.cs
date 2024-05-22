using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using userauthentication.Models;

namespace userauthentication.Repositories;

public interface IAdminRepository
{
	Task<List<User>> Users();

	Task<User?> SearchUser(string email);

	Task<GeneralResponse> Register(UserRegisterRequestAdmin request);

	Task<GeneralResponse> RemoveUser(string email);

	Task<GeneralResponse> UpdateUser(UserEditRequest request);
}
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;

namespace userauthentication.Repositories;

public interface IUserRepository
{
	UserInfoResponse UserInfo();

	Task<GeneralResponse> UpdateUserInfo(UserUpdateRequest request);
}
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;

namespace userauthentication.Services;

public interface IUserService
{
	UserInfoResponse UserInfo();

	Task<GeneralResponse> UpdateUserInfo(UserUpdateRequest request);
}
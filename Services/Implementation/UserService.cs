using userauthentication.Models;
using userauthentication.DTO.Request;
using userauthentication.DTO.Response;
using System.Security.Claims;
using userauthentication.Repositories;
using userauthentication.EntityBuilder;

namespace userauthentication.Services.Implementation;

public class UserService : IUserService
{
	private readonly IUserRepository _userrepository;
	private readonly IService _service;

	public UserService(IUserRepository userrepository, IService service)
	{
		_userrepository = userrepository;
		_service = service;
	}

	// User Info
	public UserInfoResponse UserInfo()
	{
		string userInfo = _service.getCurrentUser(ClaimTypes.Name);

		User? user = _userrepository.FindById(userInfo);

		return new UserInfoResponse(
			user.Id,
			user.Uid,
			user.Email,
			user.CreatedAt,
			user.AccountType,
			user.VerifiedAt
		);
	}

	public async Task<GeneralResponse> UpdateUserInfo(UserUpdateRequest request)
	{
		string userInfo = _service.getCurrentUser(ClaimTypes.Name);

		User? user = _userrepository.FindById(userInfo);

		if (user == null) return new GeneralResponse(false, "Unable to Update Information");
		if (_userrepository.FindByEmail(request.Email) != null && user.Email != request.Email)
			return new GeneralResponse(false, "Unable to Update Information");

		User UpdatedUser = new UserBuilder(user)
			.SetEmail(request.Email)
			.BuildUser();

		_userrepository.Update(user, UpdatedUser);
		await _userrepository.SaveAsync();

		return new GeneralResponse(true, "Information Updated Successfully.");
	}
}
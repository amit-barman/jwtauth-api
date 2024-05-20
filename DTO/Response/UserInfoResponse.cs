namespace userauthentication.DTO.Response
{
	public record UserInfoResponse(
		int Id,
		string Uid,
		string email,
		DateTime createdAt,
		string accountType,
		DateTime? verifiedAt
	);
}
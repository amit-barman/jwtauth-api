namespace userauthentication.DTO.Response
{
	public record LoginResponse(bool status, string token, string refreshToken, string message);
}
namespace userauthentication.DTO.Response
{
	public record TokenResponse(bool status, string token, string message);
}
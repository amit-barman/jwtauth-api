using System.ComponentModel.DataAnnotations;

namespace userauthentication.DTO.Request;

public class UserUpdateRequest
{
	[EmailAddress]
	public string Email { get; set; } = string.Empty;
}
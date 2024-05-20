using System.ComponentModel.DataAnnotations;

namespace userauthentication.DTO.Request;

public class UserEditRequest
{
	[Required]
	public string Uid { get; set; }

	[EmailAddress]
	public string Email { get; set; } = string.Empty;

	public string AccountType { get; set; } = string.Empty;
}
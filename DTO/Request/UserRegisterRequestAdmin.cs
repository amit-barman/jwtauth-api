using System.ComponentModel.DataAnnotations;

namespace userauthentication.DTO.Request;

public class UserRegisterRequestAdmin
{
	[Required, EmailAddress]
	public string Email { get; set; } = string.Empty;

	[Required, MinLength(6)]
	public string Password { get; set; } = string.Empty;
	
	[Required, Compare("Password")]
	public string ConfirmPassword { get; set; } = string.Empty;

	[Required]
	public string AccountType { get; set; } = string.Empty;
}
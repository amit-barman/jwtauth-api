using System.ComponentModel.DataAnnotations;

namespace userauthentication.DTO.Request
{
	public class ForgotPasswordRequest
	{
		[Required, EmailAddress]
		public string Email { get; set; }
	}
}
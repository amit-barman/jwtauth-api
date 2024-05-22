namespace userauthentication.Models;

public class User
{
	public int Id { get; set; }

	public string Uid { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public byte[] PasswordHash { get; set; }

	public byte[] PasswordSalt { get; set; }

	public DateTime CreatedAt { get; set; }

	public string AccountType { get; set; } = string.Empty;

	public string? RefreshToken { get; set; }

	public DateTime? RefreshTokenExpirationTime { get; set; }

	public string? EmailVerificationToken { get; set; }

	public DateTime? VerifiedAt { get; set; }

	public string? PasswordResetToken { get; set; }

	public DateTime? PasswordResetTokenExpires { get; set; }
}
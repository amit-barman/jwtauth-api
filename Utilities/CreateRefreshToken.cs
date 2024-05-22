using System.Security.Cryptography;

namespace userauthentication.Utilities;

public class CreateRefreshToken
{
	public static string RefreshToken()
	{
		return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}
}
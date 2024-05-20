using System.Security.Cryptography;

namespace userauthentication.Utilities;

public class CreateVerifyToken
{
	public static string CreateRandomToken()
	{
		return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
	}
}
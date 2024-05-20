using System.Security.Cryptography;

namespace userauthentication.Utilities;

public class VerifyPasswordHash
{
	public static bool VerifyHash(string password, byte[] passwordHash, byte[] passwordSalt)
	{
		using(HMACSHA512 hmac = new HMACSHA512(passwordSalt))
		{
			var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computeHash.SequenceEqual(passwordHash);
		}
	}
}
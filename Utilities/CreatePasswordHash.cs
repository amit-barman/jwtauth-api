using System.Security.Cryptography;

namespace userauthentication.Utilities;

public class CreatePasswordHash
{
	public static void CreateHash(string password, out byte[] passwordHash, 
		out byte[] passwordSalt)
	{
		using(HMACSHA512 hmac = new HMACSHA512())
		{
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}
	}
}
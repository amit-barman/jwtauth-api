using userauthentication.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace userauthentication.Utilities;

public sealed class CreateJWTToken
{
	public static string CreateToken(User user, string secretKey, int JWTExpirationTime)
	{
		List<Claim> clames = new List<Claim> {
			new Claim(ClaimTypes.Name, user.Uid),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.AccountType)
		};

		var key = new SymmetricSecurityKey(
			System.Text.Encoding.UTF8.GetBytes(secretKey)
		);

		var signingCredential = new SigningCredentials(
			key, 
			SecurityAlgorithms.HmacSha512Signature
		);

		var token = new JwtSecurityToken(
			claims: clames,
			expires: DateTime.Now.AddHours(JWTExpirationTime),
			signingCredentials: signingCredential
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
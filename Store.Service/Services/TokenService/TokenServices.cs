using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.TokenService
{
	public class TokenServices : ITokenServices
	{
		private readonly IConfiguration configuration;
		private readonly SymmetricSecurityKey key;

		public TokenServices(IConfiguration configuration )
        {
			this.configuration = configuration;
			//Creating Verify Signature	Key			hashing the key
			key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]));
		}

        public string GenerateToken(AppUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.GivenName, user.DisplayName),				
			};
			var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
			//describe the properties of a security token that will be generated,
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Issuer = configuration["Token:Issuer"],
				IssuedAt = DateTime.Now,
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = creds,
				Audience = configuration["Token:Issuer"],
			};
			//Generating Token
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}

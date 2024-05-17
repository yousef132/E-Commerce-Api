using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Session_1.Helper;
using Store.Data.Entities.IdentityEntities;
using Store.Data.StoreDbContext;
using System.Text;

namespace Session_1.Extentions
{
	public static class IdentityServiceExtentions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services,IConfiguration configuration)
		{
			var builder = services.AddIdentityCore<AppUser>();

			builder.Services.AddSingleton<JwtBearerOptions>();
			builder = new IdentityBuilder(builder.UserType, builder.Services);

			builder.AddEntityFrameworkStores<StoreIdentityDbContext>(); 
			builder.AddSignInManager<SignInManager<AppUser>>();

			// add jwt bearer Scheme
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					// enable validations on request SigningKey
					ValidateIssuerSigningKey = true,
					// comparing SigningKeys					hashing SigningKey
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
					ValidateIssuer = true,
					ValidIssuer = configuration["Token:Issuer"],
					ValidateAudience = false,

				};
			});
			return services;

		}
	}
}

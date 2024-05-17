using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using Store.Service.Services.TokenService;
using Store.Service.Services.UserService.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserService.UserService
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> userManager;
		private readonly SignInManager<AppUser> signInManager;
		private readonly ITokenServices tokenServices;

		public UserService(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			ITokenServices tokenServices)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.tokenServices = tokenServices;
		}
		public async Task<UserDto> Login(LoginDto LoginDto)
		{
			var user = await userManager.FindByEmailAsync(LoginDto.Email);
			if (user is null)
				return null;

			var result = await signInManager.CheckPasswordSignInAsync(user, LoginDto.Password, true);

			if (!result.Succeeded)
				throw new Exception("Login Faild");

			return new UserDto
			{
				DisplyName = user.DisplayName,
				Email = user.Email,
				Token = tokenServices.GenerateToken(user)
			};

		}

		public async Task<UserDto> Register(RegisterDto registerDto)
		{
			var user = await userManager.FindByEmailAsync(registerDto.Email);
			if (user is not null)
				return null;

			var appUser = new AppUser
			{
				UserName = registerDto.DisplyName,
				Email = registerDto.Email,
				DisplayName = registerDto.DisplyName
			};

			var result = await userManager.CreateAsync(appUser, registerDto.Password);

			if (!result.Succeeded)
				throw new Exception(result.Errors.Select(x => x.Description).FirstOrDefault());


			return new UserDto
			{
				DisplyName = appUser.DisplayName,
				Email = appUser.Email,
				Token = tokenServices.GenerateToken(appUser)
			};


		}

	}
}

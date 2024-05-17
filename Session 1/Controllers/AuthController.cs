using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.IdentityEntities;
using Store.Service.HandleResponses;
using Store.Service.Services.TokenService;
using Store.Service.Services.UserService.UserService;
using Store.Service.Services.UserService.UserService.Dtos;
using System.Security.Claims;

namespace Session_1.Controllers
{

	public class AuthController : BaseController
	{
		private readonly IUserService userService;
		private readonly UserManager<AppUser> userManager;

		public AuthController(IUserService userService,UserManager<AppUser> userManager)
        {
			this.userService = userService;
			this.userManager = userManager;
		}

		[HttpPost]

		public  async Task<ActionResult<UserDto>> Login(LoginDto userDto)
		{
			var user = await userService.Login(userDto);
			if(user is null)
				return Unauthorized(new CustomException(401));
			return user;

		}
		[HttpPost]

		public  async Task<ActionResult<UserDto>> Register(RegisterDto userDto)
		{
			var user = await userService.Register(userDto);
			if(user is null)
				return BadRequest(new CustomException(400,"Email Already Exists"));
			return user;
		}

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<UserDto>> GetCurrentUserDetails()
		{
			// get user email from token
			var Email = User?.FindFirstValue(ClaimTypes.Email);
			//var Email = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

			var user = await userManager.FindByEmailAsync(Email);

			return new UserDto
			{
				DisplyName = user.DisplayName,
				Email = user.Email,
			};
		}

	}
}

using Store.Service.Services.UserService.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserService.UserService
{
    public interface IUserService
	{
		Task<Dtos.UserDto> Register(Dtos.RegisterDto registerDto);
		Task<Dtos.UserDto> Login(Dtos.LoginDto registerDto);

	}
}

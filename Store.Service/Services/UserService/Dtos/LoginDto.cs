using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.UserService.UserService.Dtos
{
    public class LoginDto
	{

		[EmailAddress]
		[Required]
		public string Email { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")]
		public string Password { get; set; }
	}
}

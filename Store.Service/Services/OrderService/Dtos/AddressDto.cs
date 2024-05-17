using Store.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.OrderService.Dtos
{
	public class AddressDto
	{
		[Required] 
		public string FName { get; set; }
		[Required]
		public string LName { get; set; }
		[Required]
		public string Street { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string ZipCode { get; set; }
	}
}
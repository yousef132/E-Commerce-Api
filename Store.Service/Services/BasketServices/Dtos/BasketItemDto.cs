using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketServices.Dtos
{
	public class BasketItemDto
	{

		[Range(0,int.MaxValue)]
		public int Id { get; set; }
		public string Name { get; set; }

		public string Brand { get; set; }
		[Range(0.1,double.MaxValue,ErrorMessage ="Price Must Be Greater Than 0")]
		public double Price { get; set; }
		public string PictureUrl { get; set; }
		public string Type { get; set; }
		[Range(1,10,ErrorMessage ="Quantity Must Be Between 1 and 10 Pieces")]
		public int Quantity { get; set; }
	}
}

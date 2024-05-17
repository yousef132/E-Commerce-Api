﻿using System.ComponentModel.DataAnnotations;

namespace Store.Data.Entities.IdentityEntities
{
	public class Address
	{
		public long Id { get; set; }	

		public string FName { get; set; }
		public string LName { get; set; }
		public string Street { get; set; }
		public string City { get; set; }	

		public string ZipCode { get; set; }
		[Required]
		public string AppUserId { get; set; }	

		public AppUser AppUser { get; set; }
	}
}
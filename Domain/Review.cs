using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReviews.Models
{
	public class Review
	{
		public int Id { get; set; }
		public string Author { get; set; }
		public string Restaurant { get; set; }
		public string ReviewText { get; set; }
	}
}
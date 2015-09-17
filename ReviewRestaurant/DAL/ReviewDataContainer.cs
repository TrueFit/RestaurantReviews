using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    /// <summary>
    /// Data container class with very simple validation for restaurant reviews.
    /// </summary>
    public class ReviewDataContainer
    {
            private string restaurantName;
            private string username;
            private string reviewText;
            private string ratingDescription;
            private string cityName;
            private string stateName;
            private int reviewID;
            private DateTime reviewDate;
            
            [Required]
            [StringLength(50)]
            public string RestaurantName
            {
                get { return restaurantName; }
                set { restaurantName = value; }
            }
            
            [Required]
            [StringLength(50)]
            public string Username
            {
                get { return username; }
                set { username = value; }
            }

            [Required]
            [StringLength(1000)]
            public string ReviewText
            {
                get { return reviewText; }
                set { reviewText = value; }
            }

            [Required]
            [StringLength(50)]
            public string RatingDescription
            {
                get { return ratingDescription; }
                set { ratingDescription = value; }
            }

            [StringLength(50)]
            public string CityName
            {
                get { return cityName; }
                set { cityName = value; }
            }

            [StringLength(50)]
            public string StateName
            {
                get { return stateName; }
                set { stateName = value; }
            }

            public int ReviewID
            {
                get { return reviewID; }
                set { reviewID = value; }
            }

            public DateTime ReviewDate
            {
                get { return reviewDate; }
                set { reviewDate = value; }
            }
        }
    }


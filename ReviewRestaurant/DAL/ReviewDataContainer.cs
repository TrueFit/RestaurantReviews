using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ReviewDataContainer
    {
            private string restaurantName;
            private string username;
            private string reviewText;
            private string ratingDescription;
            private string cityName;
            private string stateName;
            private int reviewID;

            public string RestaurantName
            {
                get { return restaurantName; }
                set { restaurantName = value; }
            }

            public string Username
            {
                get { return username; }
                set { username = value; }
            }

            public string ReviewText
            {
                get { return reviewText; }
                set { reviewText = value; }
            }

            public string RatingDescription
            {
                get { return ratingDescription; }
                set { ratingDescription = value; }
            }

            public string CityName
            {
                get { return cityName; }
                set { cityName = value; }
            }

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
        }
    }


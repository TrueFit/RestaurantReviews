using RestaurantReviews.Core;
using RestaurantReviews.Data;
using System;
using System.Data.Entity.Spatial;
using System.Xml;

namespace RestaurantReviews.Service
{
    public class RestaurantService : BaseService
    {
        private RestaurantRepository _restaurantRepo;

        public RestaurantService()
        {
            _restaurantRepo = new RestaurantRepository();
        }

        public ServiceCallResult AddNewRestaurant(Restaurant entity)
        {
            entity.Validate();

            //get geo code from google.
            setRestaurantGeography(ref entity);

            ServiceCallResult srv = new ServiceCallResult();
            srv.ValidationErrors.AddRange(entity.ValidationErrors);

            if (entity.IsValid)
            {
                try
                {
                    int? entityID = _restaurantRepo.AddEntity(entity);
                    if (entityID == null)
                        srv.ValidationErrors.Add("Error inserting the record into the database");
                    else
                        srv.ResultObject = entityID;
                }
                catch (Exception ex)
                {
                    srv.ValidationErrors.Add("Database error: " + ex.Message);
                }
            }
            return srv;
        }

        //this will get the restaurant and the reviews for the restaurant
        public ServiceCallResult GetRestaurantById(int Id)
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                Restaurant entity = _restaurantRepo.GetById(Id);
                srv.ResultObject = _restaurantRepo.GetById(Id);
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

        public ServiceCallResult GetAllRestaurants()
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                srv.ResultObject = _restaurantRepo.GetAll();
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

        public ServiceCallResult GetRestaurantsForCity(string city)
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                srv.ResultObject = _restaurantRepo.GetRestaurantsByCity(city);
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

        public ServiceCallResult GetRestaurantsWhereNameContains(string name)
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                srv.ResultObject = _restaurantRepo.GetRestaurantsWithNameContains(name);
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

        public ServiceCallResult GetRestaurantsAroundMe(int distance, double longitude, double latitude)
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                srv.ResultObject = _restaurantRepo.GetRestaurantsAroundMe(distance, longitude, latitude);
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

        public ServiceCallResult CreateRestaurantReview(RestaurantReview restReview)
        {
            restReview.Validate();

            ServiceCallResult srv = new ServiceCallResult();
            srv.ValidationErrors.AddRange(restReview.ValidationErrors);

            if (restReview.IsValid)
            {
                try
                {
                    int? entityID = _restaurantRepo.CreateRestaurantReview(restReview);
                    if (entityID == null)
                        srv.ValidationErrors.Add("Error inserting the record into the database");
                    else
                        srv.ResultObject = entityID;
                }
                catch (Exception ex)
                {
                    srv.ValidationErrors.Add("Database error: " + ex.Message);
                }
            }
            return srv;
        }

        private void setRestaurantGeography(ref Restaurant entity)
        {
            try
            {
                string restAddr = string.Format("{0} {1} {2} {3}", entity.Street, entity.City, entity.State, entity.PostalCode);
                XmlDocument xmlDoc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();

                XmlReader rdr = XmlReader.Create("https://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address=" + restAddr, settings);
                xmlDoc.Load(rdr);

                string gStatus = xmlDoc.SelectSingleNode("GeocodeResponse/status").InnerText;
                if (gStatus == "OK" && xmlDoc.SelectNodes("GeocodeResponse/result").Count > 0)
                {
                    XmlNode firstResult = xmlDoc.SelectSingleNode("GeocodeResponse/result");
                    XmlNode geoLocNode = firstResult.SelectSingleNode("geometry/location");
                    string latVal = geoLocNode.SelectSingleNode("lat").InnerText;
                    string longVal = geoLocNode.SelectSingleNode("lng").InnerText;
                    //string locType = firstResult.SelectSingleNode("geometry/location_type").InnerText;

                    DbGeography geo = DbGeography.FromText(string.Format("POINT({0} {1})", longVal, latVal));
                    entity.GeoLocation = geo;
                }
            }
            catch (Exception ex)
            {
                //error getting GEO location. 
            }

        }

    }

   
}

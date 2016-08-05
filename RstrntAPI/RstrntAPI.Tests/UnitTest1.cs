using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using RstrntAPI.DataAccess.Models;
using System.Collections.Generic;
using RstrntAPI.Business.Repositories;

namespace RstrntAPI.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var table = new City();
            //Assert.IsNotNull(table);
            //var cities = table.All();
            //Assert.IsNotNull(cities);

            //var rTable = new DataAccess.Models.Restaurants();
            //var restaurants = rTable.Query("SELECT Restaurants.id, Restaurants.name, Locations.street_address, City.name as cityname, Locations.city_id FROM Restaurants INNER JOIN Locations ON Locations.restaurant_id = Restaurants.id INNER JOIN City ON City.id = Locations.city_id");
            //var filteredResults = restaurants.Where(x => x.name.Contains("Pittsburgh") || x.street_address.Contains("Pittsburgh"));
            var t = new RestaurantRepository();
            var thing = t.Create(new DTO.RestaurantDTO { Name = "Generic Food Place" });
            Assert.IsNotNull(t);
        }
    }
}

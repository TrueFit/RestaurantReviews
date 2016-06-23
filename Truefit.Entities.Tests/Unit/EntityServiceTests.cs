using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RestaurantReviews.Data;
using RestaurantReviews.Data.Models;
using RestaurantReviews.Data.Repositories;

namespace Truefit.Entities.Tests.Unit
{
    [TestFixture]
    public class EntityServiceTests
    {
        private Mock<ICityRepository> _cityRepository;
        private Mock<IEntityRepository> _entityRepository;
        private IEntityService _entityService;

        [SetUp]
        public void Setup()
        {
            this._cityRepository = new Mock<ICityRepository>();
            this._entityRepository = new Mock<IEntityRepository>();
            this._entityService = new EntityService(this._cityRepository.Object, this._entityRepository.Object);
        }

        [Test]
        public async Task GetCity_Is_Repo_Passthrough()
        {
            var guid = Guid.NewGuid();
            var expected = new CityModel();

            this._cityRepository.Setup(x => x.GetByGuid(guid)).ReturnsAsync(expected);
            var actual = await this._entityService.GetCity(guid);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAllCities_Is_Repo_Passthrough()
        {
            var expected = new[] { new CityModel() };

            this._cityRepository.Setup(x => x.GetAll()).ReturnsAsync(expected);
            var actual = await this._entityService.GetAllCities();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetEntity_Is_Repo_Passthrough()
        {
            var guid = Guid.NewGuid();
            var expected = new EntityModel();

            this._entityRepository.Setup(x => x.GetByGuid(guid)).ReturnsAsync(expected);
            var actual = await this._entityService.GetEntity(guid);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetEntities_Retrieves_From_Repo_By_City_Guid()
        {
            var guid = Guid.NewGuid();
            var type = string.Empty;
            var city = new CityModel {Guid = guid};
            var expected = new[] {new EntityModel()};

            this._entityRepository.Setup(x => x.GetByCityAndType(guid, type)).ReturnsAsync(expected);
            var actual = await this._entityService.GetEntities(city, type);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task InsertUserEntity_Should_Set_NeedsApproved_To_True()
        {
            var entity = new EntityModel();
            await this._entityService.InsertUserEntity(entity);
            this._entityRepository.Verify(x => x.Insert(It.Is<EntityModel>(e => e.NeedsReviewed)));
        }

        [Test]
        public async Task InsertUserEntity_Should_Set_IsActive_To_False()
        {
            var entity = new EntityModel();
            await this._entityService.InsertUserEntity(entity);
            this._entityRepository.Verify(x => x.Insert(It.Is<EntityModel>(e => !e.IsActive)));
        }
    }
}

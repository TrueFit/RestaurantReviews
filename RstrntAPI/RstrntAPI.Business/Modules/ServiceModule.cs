using Ninject.Modules;
using RstrntAPI.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.Business.Modules
{
    class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRestaurantService>().To<RestaurantService>().InSingletonScope();
            Bind<ICityService>().To<CityService>().InSingletonScope();
            Bind<ILocationService>().To<LocationService>().InSingletonScope();
            Bind<IReviewService>().To<ReviewService>().InSingletonScope();
            Bind<IUserService>().To<UserService>().InSingletonScope();
        }
    }
}

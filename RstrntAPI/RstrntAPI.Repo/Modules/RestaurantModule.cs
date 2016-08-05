using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using RstrntAPI.Business.Repositories;

namespace RstrntAPI.Business.Modules
{
    class RestaurantModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRestaurantRepository>().To<RestaurantRepository>().InSingletonScope();
            Bind<ICityRepository>().To<CityRepository>().InSingletonScope();
            Bind<ILocationRepository>().To<LocationRepository>().InSingletonScope();
            Bind<IReviewRepository>().To<ReviewRepository>().InSingletonScope();
            Bind<IUserRepository>().To<UserRepository>().InSingletonScope();
        }
    }
}

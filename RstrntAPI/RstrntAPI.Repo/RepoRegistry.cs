using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using RstrntAPI.Business.Modules;

namespace RstrntAPI.Business
{
    public class RepoRegistry
    {
        private static readonly IKernel kernel = new StandardKernel(
                new NinjectModule[]
                {
                    new RestaurantModule()
                }
            );

        public static T Get<T>()
        {
            return kernel.Get<T>();
        }
    }
}

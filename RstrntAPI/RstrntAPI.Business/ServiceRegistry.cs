using Ninject;
using Ninject.Modules;
using RstrntAPI.Business.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.Business
{
    public class ServiceRegistry
    {
        private static readonly IKernel kernel = new StandardKernel(
                new NinjectModule[]
                {
                    new ServiceModule()
                }
            );

        public static T Get<T>()
        {
            return kernel.Get<T>();
        }
    }
}

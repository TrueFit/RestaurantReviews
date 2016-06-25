using System.Web.Http;
using Biggy.Data.Json;
using Ninject.Web.WebApi;
using Truefit.Entities;
using Truefit.Entities.Repositories;
using Truefit.Reviews;
using Truefit.Reviews.Repositories;
using Truefit.Users;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Truefit.Api.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Truefit.Api.NinjectWebCommon), "Stop")]
namespace Truefit.Api
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper _bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            _bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            _bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();
            kernel.Bind<IReviewRepository>().To<ReviewJsonRepository>().InSingletonScope();
            kernel.Bind<IReviewService>().To<ReviewService>().InRequestScope();
            kernel.Bind<ICityRepository>().To<CityJsonRepository>().InSingletonScope();
            kernel.Bind<IEntityRepository>().To<EntityJsonRepository>().InSingletonScope();
            kernel.Bind<IEntityService>().To<EntityService>().InRequestScope();
            kernel.Bind<JsonDbCore>().ToConstant(new JsonDbCore(AppDomain.CurrentDomain.BaseDirectory + "Data", "demo"));
        }        
    }
}

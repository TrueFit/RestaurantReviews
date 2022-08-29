using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NoREST.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Tests.Utilities
{
    public static class TestServiceRegistry
    {
        //public static IServiceProvider BuildServiceProvider()
        //{
        //    // We rely on there being a single object in tests... thus register all as singletons
        //    var services = new ServiceCollection();

        //    services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ServiceRegistry))));
           
        //    services.AddSingleton(x => new Mock<IHttpContextAccessor>().Object);

        //}

        public static IMapper GetAutoMapperMapper()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ServiceRegistry))));
            return services.BuildServiceProvider().GetService<IMapper>();
        }
    }

    //public class TestLogger<T> : ILogger<T>
    //{
    //    public IDisposable BeginScope<TState>(TState state)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool IsEnabled(LogLevel logLevel)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

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
        public static IMapper GetAutoMapperMapper()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(ServiceRegistry))));
            return services.BuildServiceProvider().GetService<IMapper>();
        }
    }
}

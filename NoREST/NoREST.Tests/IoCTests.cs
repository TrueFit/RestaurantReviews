using Amazon.CognitoIdentityProvider;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NoREST.Api;
using NoREST.Api.Auth;
using NoREST.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NoREST.Tests
{
    public class IoCTests
    {
        [Fact]
        public void AllControllers_CanBeInstantiated_FromServiceRegistry()
        {
            var serviceCollection = new ServiceCollection();
            var mockConfig = new Mock<IConfiguration>(MockBehavior.Loose);

            ServiceRegistry.RegisterServices(serviceCollection, mockConfig.Object);
            serviceCollection.AddTransient(x => new Mock<IAmazonCognitoIdentityProvider>().Object);
            var controllerTypes = ReflectionTricks.GetAllTypesOfType(typeof(ServiceRegistry), typeof(ControllerBase));

            foreach (var ctr in controllerTypes)
            {
                serviceCollection.AddTransient(ctr);
            }

            var builder = serviceCollection.BuildServiceProvider();

            foreach (var controllerType in controllerTypes)
            {
                builder
                    .Invoking(b => b.GetService(controllerType))
                    .Should()
                    .NotThrow($"all dependencies of {controllerType.Name} (recursively) should be registered in {nameof(ServiceRegistry.RegisterServices)}");
            }
        }

        [Fact]
        public void CognitoTokenValidator_CanBeInstantiated_FromServiceCollection()
        {
            var serviceCollection = new ServiceCollection();
            var mockConfig = new Mock<IConfiguration>(MockBehavior.Loose);

            ServiceRegistry.RegisterServices(serviceCollection, mockConfig.Object);
            serviceCollection.AddTransient(x => new Mock<IAmazonCognitoIdentityProvider>().Object);

            var builder = serviceCollection.BuildServiceProvider();

            builder.Invoking(b => b.GetService<ICognitoTokenValidator>())
                .Should()
                .NotThrow("This one gets resolved from the HttpContext inside an attribute (and not in its ctor) and therefore isn't covered by the controllers test.");
        }
    }
}

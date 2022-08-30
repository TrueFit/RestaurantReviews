using Amazon.CognitoIdentityProvider;
using Microsoft.EntityFrameworkCore;
using NoREST.Api.Auth;
using NoREST.DataAccess;
using NoREST.DataAccess.Repositories;
using NoREST.Domain;
using NoREST.Models.DomainModels;
using System.Reflection;

namespace NoREST.Api
{
    public static class ServiceRegistry
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            services.AddControllers();
            services.AddLogging();
            if (config["DbContextType"] == "InMemory")
            {
                services.AddDbContextFactory<NoRESTContext>(opt =>
                    opt.UseInMemoryDatabase("NoREST"));
            }
            else
            {
                services.AddDbContextFactory<NoRESTContext>(
                    opt => opt.UseSqlServer(config.GetConnectionString("NoRestConnection")));
            }

            services.AddHttpContextAccessor();
            services.Add(new ServiceDescriptor(typeof(IConfiguration), config));
            services.Add(new ServiceDescriptor(typeof(ICognitoPoolInfo), new CognitoPoolInfo(config)));
            
            RegisterApplicationServices(services);
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IKeyIdFetcher, KeyIdFetcher>();
            services.AddSingleton<IKeyIdHandler, KeyIdHandler>();
            services.AddTransient<IIdentityProviderService, IdentityProviderService>();
            services.AddTransient<IAmazonCognitoIdentityProvider>(x =>
            {
                var config = x.GetRequiredService<ICognitoPoolInfo>();
                return new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(config.Region));
            });
            services.AddTransient<ICognitoTokenValidator, CognitoTokenValidator>();
            services.AddTransient<IUserLogic, UserLogic>();
            services.AddTransient<IRestaurantLogic, RestaurantLogic>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBanManager, BanManager>();
            services.AddTransient<IAuditLogic, AuditLogic>();
            services.AddTransient<IPermissionLogic, PermissionLogic>();            
        }

    }
}

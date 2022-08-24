using Microsoft.EntityFrameworkCore;
using NoREST.DataAccess;

namespace NoREST
{
    public static class ServiceRegistry
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.Add(new ServiceDescriptor(typeof(IConfiguration), config));
            services.AddDbContext<NoRESTContext>(opt =>
                opt.UseInMemoryDatabase("NoREST"));


        }
    }
}

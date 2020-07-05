using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Repositories;
using WebApi.Persistent.Repositories;

namespace WebApi.Persistent
{
    public static class ServiceCollectionSetup
    {
        public static void AddAppDbContext(this IServiceCollection services) =>
            services.AddDbContext<AppDbContext>(options => {
                options.UseInMemoryDatabase("AppDb");
            });

        public static void AddAppRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        } 
    }
}

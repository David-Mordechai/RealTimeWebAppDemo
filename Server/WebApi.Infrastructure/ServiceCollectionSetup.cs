using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;
using WebApi.Application.Channels;
using WebApi.Infrastructure.BackgroundServices;

namespace WebApi.Infrastructure
{
    public static class ServiceCollectionSetup
    {
        public static void AddAppInfrastructureServices(this IServiceCollection services)
        {
            services.AddHostedService<NotificationDispatcher>();
            services.AddSingleton(Channel.CreateUnbounded<string>());
            services.AddSingleton<INotificationChannel, NotificationChannel>();
            services.AddSingleton<IMessageProcesser, MessageProcesser>();
        }
    }
}

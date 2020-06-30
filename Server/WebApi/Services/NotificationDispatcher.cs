using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using WebApi.Services.Data.Entities;

namespace WebApi.Services
{
    public class NotificationDispatcher : BackgroundService
    {
        private readonly ILogger<NotificationDispatcher> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Channel<string> _channel;
        private readonly IServiceProvider _serviceProvider;

        public NotificationDispatcher(
            ILogger<NotificationDispatcher> logger, 
            IHttpClientFactory httpClientFactory,
            Channel<string> channel, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _channel = channel;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested is false && 
                _channel.Reader.Completion.IsCompleted is false)
            {
                // read from channel, when is something to read then goes to try block
                var msg = await _channel.Reader.ReadAsync();
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    
                    var database = scope.ServiceProvider.GetRequiredService<Database>();
                    
                    if (!await database.Users.AnyAsync(stoppingToken))
                    {
                        database.Users.Add(new User());
                        await database.SaveChangesAsync(stoppingToken);
                    }

                    var user = await database.Users.FirstOrDefaultAsync();

                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetStringAsync("https://docs.microsoft.com/en-us/dotnet/core/");
                    user.Message = $"{msg} - {response}";

                    await database.SaveChangesAsync(stoppingToken);
                }
                catch (Exception e)
                {   
                    _logger.LogError(e, "notification failed");
                }
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using WebApi.Domain.Entities;
using System.Net.Http;
using WebApi.Application.Repositories;

namespace WebApi.Infrastructure.BackgroundServices
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
                    _logger.LogInformation("Background Service started for message {message}", msg);
                    using var scope = _serviceProvider.CreateScope();

                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    var users = await userRepository.GetUsers();

                    if (users.Any() is false)
                    {
                        await userRepository.AddUserAsync(new User());
                    }

                    var user = await userRepository.GetUserById(1);

                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetStringAsync("https://docs.microsoft.com/en-us/dotnet/core/");

                    user.Message = $"{msg} - {response}";
                    await userRepository.UpdateUser(user);
                    await Task.Delay(10000);
                    _logger.LogInformation("Background Service ended for message {message}", msg);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "notification failed");
                }
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Domain.Entities;
using System.Net.Http;
using WebApi.Application.Repositories;
using WebApi.Application.Channels;

namespace WebApi.Infrastructure.BackgroundServices
{
    public class MessageProcesser : IMessageProcesser
    {
        private readonly ILogger<MessageProcesser> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public MessageProcesser(
            ILogger<MessageProcesser> logger,
            IServiceProvider serviceProvider,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
        }

        public async void Process(string msg)
        {
            _logger.LogInformation("Background Service started for message {message}", msg);
            var scope = _serviceProvider.CreateScope();

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
    }
}

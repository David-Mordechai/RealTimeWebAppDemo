using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using WebApi.Application.Channels;

namespace WebApi.Infrastructure.BackgroundServices
{
    public partial class NotificationDispatcher : BackgroundService
    {
        private readonly ILogger<NotificationDispatcher> _logger;
        private readonly Channel<string> _channel;
        private readonly IMessageProcesser _messageProcesser;

        public NotificationDispatcher(
            ILogger<NotificationDispatcher> logger,
            Channel<string> channel,
            IMessageProcesser messageProcesser)
        {
            _logger = logger;
            _channel = channel;
            _messageProcesser = messageProcesser;
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
                    _messageProcesser.Process(msg);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "notification failed");
                }
            }
        }

    }
}

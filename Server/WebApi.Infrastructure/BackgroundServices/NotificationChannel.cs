using System.Threading.Channels;
using System.Threading.Tasks;
using WebApi.Application.Channels;

namespace WebApi.Infrastructure.BackgroundServices
{
    public class NotificationChannel : INotificationChannel
    {
        private readonly Channel<string> _channel;

        public NotificationChannel(Channel<string> channel)
        {
            _channel = channel;
        }

        public async Task SendAsync(string message)
        {
            await _channel.Writer.WriteAsync(message);
        }
    }
}

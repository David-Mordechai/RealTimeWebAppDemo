using System.Threading.Tasks;

namespace WebApi.Application.Channels
{
    public interface INotificationChannel
    {
        Task SendAsync(string message);
    }
}

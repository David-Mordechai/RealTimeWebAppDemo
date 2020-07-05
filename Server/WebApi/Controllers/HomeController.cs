using System.Net.Http;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Application.Channels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotificationChannel _notificationChannel;

        public HomeController(ILogger<HomeController> logger, INotificationChannel notificationChannel)
        {
            _logger = logger;
            _notificationChannel = notificationChannel;
        }

        [Route("[action]")]
        public async Task<bool> Send([FromQuery]string message)
        {
            var messages = new[] { "a", "b", "c", "d", "e" };
            foreach (var msg in messages)
            {
                await _notificationChannel.SendAsync(msg);
            }
            
            return true;
        }
    }
}

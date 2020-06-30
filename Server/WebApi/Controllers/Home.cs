using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Home : ControllerBase
    {
        private readonly ILogger<Home> _logger;

        public Home(ILogger<Home> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Send([FromServices] Channel<string> channel)
        {
            await channel.Writer.WriteAsync("Hello");
            return true;
        }
    }
}

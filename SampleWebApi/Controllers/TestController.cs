using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Info log test!");
            _logger.LogError("Error log test!");
            _logger.LogWarning("Warning log test!");

            return Ok();
        }
    }
}

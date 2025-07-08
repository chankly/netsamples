using Microsoft.AspNetCore.Mvc;

namespace HSoft.NetSamples.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExceptionsController : ControllerBase
    {

        private readonly ILogger<ExceptionsController> _logger;

        public ExceptionsController(ILogger<ExceptionsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == 1)
            {
                throw new BadHttpRequestException("id parameter can not be 1");
            }

            if (id == 2)
            {
                throw new AccessViolationException();
            }

            return Ok();
        }
    }
}

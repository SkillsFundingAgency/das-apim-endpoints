using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AODP.Api;

namespace SFA.DAS.Aodp.Api.Controllers.FormBuilder
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FormsController> _logger;

        public PingController(IMediator mediator, ILogger<FormsController> logger) : base(mediator, logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("/Ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeController> _logger;

        public ApprenticeController(IMediator mediator, ILogger<ApprenticeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeQuery { ApprenticeId = id });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}

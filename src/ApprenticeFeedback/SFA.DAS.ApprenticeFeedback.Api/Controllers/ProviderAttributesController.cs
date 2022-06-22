using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetAttributes;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    //Needs to be renamed to /Attributes and corresponding endpoint in 
    //AppFeedback Web when both projects are next changed
    [ApiController]
    [Route("[controller]/")]
    public class ProviderAttributesController : ControllerBase
    {
        private readonly ILogger<ProviderAttributesController> _logger;
        private readonly IMediator _mediator;

        public ProviderAttributesController(
            ILogger<ProviderAttributesController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("/provider-attributes")]
        public async Task<IActionResult> GetProviderAttributes()
        {
            try
            {
                var result = await _mediator.Send(new GetAttributesQuery());

                if(result.Attributes?.Count == 0)
                {
                    return NotFound();
                }

                return Ok(result.Attributes);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting provider attributes.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetProviderAttributes;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
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
                var result = await _mediator.Send(new GetProviderAttributesQuery());

                if(result.ProviderAttributes?.Count == 0)
                {
                    return NotFound();
                }

                return Ok(result.ProviderAttributes);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting provider attributes.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

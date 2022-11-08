using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetAttributes;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AttributesController : ControllerBase
    {
        private readonly ILogger<AttributesController> _logger;
        private readonly IMediator _mediator;

        public AttributesController(
            ILogger<AttributesController> logger,
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
                var result = await _mediator.Send(new GetAttributesQuery() { AttributeType = "Feedback" });

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

        [HttpGet("/exitsurvey-attributes")]
        public async Task<IActionResult> GetExitSurveyAttributes()
        {
            try
            {
                var result = await _mediator.Send(new GetAttributesQuery() { AttributeType = "ExitSurvey_v2" });

                if (result.Attributes?.Count == 0)
                {
                    return NotFound();
                }

                return Ok(result.Attributes);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting exit survey attributes.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAttributes;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("attributes/")]
    public class AttributesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AttributesController> _logger;

        public AttributesController(IMediator mediator, ILogger<AttributesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetAttributesQuery());

                if (result.Attributes != null)
                {
                    return Ok(result.Attributes);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve attributes.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

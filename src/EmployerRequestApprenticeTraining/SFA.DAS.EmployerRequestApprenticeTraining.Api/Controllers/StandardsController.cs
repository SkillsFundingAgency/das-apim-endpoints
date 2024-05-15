using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class StandardsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StandardsController> _logger;

        public StandardsController(IMediator mediator, ILogger<StandardsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{standardId}")]
        public async Task<IActionResult> Get(string standardId)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardQuery { StandardId = standardId });
                return Ok(result.Standard);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve standard with StandardId: {standardId}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

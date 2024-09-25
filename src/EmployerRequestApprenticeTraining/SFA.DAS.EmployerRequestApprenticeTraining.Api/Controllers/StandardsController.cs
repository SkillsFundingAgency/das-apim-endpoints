using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CacheStandard;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.RefreshStandards;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveStandards;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("standards/")]
    public class StandardsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StandardsController> _logger;

        public StandardsController(IMediator mediator, ILogger<StandardsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{standardReference}")]
        public async Task<IActionResult> Get(string standardReference)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardQuery { StandardReference = standardReference });

                if (result.Standard != null)
                {
                    return Ok(result.Standard);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve standard for {StandardReference}", standardReference);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{standardLarsCode}")]
        public async Task<IActionResult> Cache(string standardLarsCode)
        {
            try
            {
                var result = await _mediator.Send(new CacheStandardCommand { StandardLarsCode = standardLarsCode });

                if (result.Standard != null)
                {
                    return Ok(result.Standard);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to cache standard for {StandardLarsCode}", standardLarsCode);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("refresh")]
        public async Task<IActionResult> RefreshStandards()
        {
            try
            {
                var standards = await _mediator.Send(new GetActiveStandardsQuery());

                await _mediator.Send(new RefreshStandardsCommand 
                { 
                    Standards = standards.Standards,
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to refresh standards");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.Application.Queries.GetTrainingProviderSearch;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EmployerFeedbackController : ControllerBase
    {
        private readonly ILogger<EmployerFeedbackController> _logger;
        private readonly IMediator _mediator;

        public EmployerFeedbackController(IMediator mediator, ILogger<EmployerFeedbackController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetTrainingProviderSearch(long accountId, Guid userRef)
        {
            try
            {
                var trainingProviderSearch = await _mediator.Send(new GetTrainingProviderSearchQuery(accountId, userRef));
                return Ok(trainingProviderSearch);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error get training provider search results.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

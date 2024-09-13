using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("provider-responses/")]
    public class ProviderResponsesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderResponsesController> _logger;

        public ProviderResponsesController(IMediator mediator, ILogger<ProviderResponsesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{providerResponseId}/confirmation")]
        public async Task<IActionResult> GetProviderResponseConfirmation(Guid providerResponseId)
        {
            try
            {
                var result = await _mediator.Send(new GetProviderResponseConfirmationQuery
                {
                    ProviderResponseId = providerResponseId
                });

                var model = (ProviderResponseConfirmation)result;
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve provider response confirmation");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

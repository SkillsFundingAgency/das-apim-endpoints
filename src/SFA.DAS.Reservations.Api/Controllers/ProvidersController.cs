using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.Providers.Queries.GetProvider;

namespace SFA.DAS.Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProvidersController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public ProvidersController(
            ILogger<TrainingCoursesController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> GetProvider(int ukPrn)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetProviderQuery{Ukprn = ukPrn});

                var model = (GetProviderResponse) queryResult.Provider;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get training provider, UKPRN: [{ukPrn}]");
                return BadRequest();
            }
        }
    }
}
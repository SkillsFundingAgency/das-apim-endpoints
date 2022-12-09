using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider;

namespace SFA.DAS.RoatpProviderModeration.Api.Controllers
{
    [Route("[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly ILogger<ProvidersController> _logger;
        private readonly IMediator _mediator;

        public ProvidersController(ILogger<ProvidersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetProvider([FromRoute] int ukprn)
        {
            var providerResult = await _mediator.Send(new GetProviderQuery(ukprn));

            if (providerResult == null)
            {
                _logger.LogError("Provider not found for ukprn {ukprn}", ukprn);
                return NotFound();
            }

            return Ok(providerResult);
        }
    }
}

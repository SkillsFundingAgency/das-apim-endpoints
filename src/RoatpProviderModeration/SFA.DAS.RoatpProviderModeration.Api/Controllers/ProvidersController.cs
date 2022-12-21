using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpProviderModeration.Application.Commands.UpdateProviderDescription;
using SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.RoatpProviderModeration.Api.Controllers
{
    [ApiController]
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

        [HttpPost]
        [Route("{ukprn}/update-provider-description")]
        public async Task<IActionResult> UpdateProviderDescription([FromRoute] int ukprn, UpdateProviderDescriptionCommand command)
        {
            _logger.LogInformation("Outer API: Request to update provider description for ukprn: {ukprn}", ukprn);
            command.Ukprn = ukprn;
            try
            {
                await _mediator.Send(command);
            }
            catch (HttpRequestContentException ex)
            {
                _logger.LogError(ex, "Outer API: Failed request to update provider description for ukprn: {ukprn} with HttpStatusCode: {httpstatuscode}", ukprn, ex.StatusCode);
                return new StatusCodeResult((int)ex.StatusCode);
            }
            return NoContent();
        }
    }
}

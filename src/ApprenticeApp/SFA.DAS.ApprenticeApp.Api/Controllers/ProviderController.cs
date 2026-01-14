using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Queries.GetActiveStandards;
using SFA.DAS.ApprenticeApp.Application.Queries.GetRoatpProviders;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    [Route("providers")]
    public class ProviderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProviderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("registeredProviders")]
        public async Task<IActionResult> GetRegisteredProviders()
        {
            var result = await _mediator.Send(new GetRoatpProvidersQuery());

            return Ok(result);
        }

        [HttpGet]
        [Route("activeStandards")]
        public async Task<IActionResult> GetActiveStandards()
        {
            var result = await _mediator.Send(new GetActiveStandardsQuery());

            return Ok(result.Courses);
        }
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProviders;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProviderController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderController> _logger;

        public ProviderController(IMediator mediator, ILogger<ProviderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{apprenticeId}")]
        public async Task<ActionResult<GetApprenticeTrainingProvidersResult>> GetApprenticeTrainingProviders([FromRoute] GetApprenticeTrainingProvidersQuery request)
            => await _mediator.Send(request);

    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProvider;
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

        [HttpGet("{ApprenticeId}")]
        public async Task<ActionResult<GetApprenticeTrainingProvidersResult>> GetApprenticeTrainingProviders([FromRoute] GetApprenticeTrainingProvidersQuery request)
        {
            _logger.LogDebug($"Begin Get Apprentice Training Providers for ApprenticeId: {request?.ApprenticeId}");
            
            var result = await _mediator.Send(request);
            _logger.LogDebug($"End Get Apprentice Training Providers for ApprenticeId: {request?.ApprenticeId}");
            return result;
        }

        [HttpGet("{ApprenticeId}/{Ukprn}")]
        public async Task<ActionResult<GetApprenticeTrainingProviderResult>> GetApprenticeTrainingProviders([FromRoute] GetApprenticeTrainingProviderQuery request)
        {
            _logger.LogDebug($"Begin Get Apprentice Training Provider for ApprenticeId: {request?.ApprenticeId}, Ukprn: {request?.Ukprn}");

            var result = await _mediator.Send(request);
            _logger.LogDebug($"End Get Apprentice Training Provider for ApprenticeId: {request?.ApprenticeId}, Ukprn: {request?.Ukprn}");
            return result;
        }


    }
}
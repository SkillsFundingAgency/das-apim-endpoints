using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget;
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
        {
            _logger.LogDebug($"Begin Get Apprentice Training Providers for ApprenticeId: {request?.ApprenticeId}");
            // Endpoint updates the relevant targets first if they need to be updated by the Learner Endpoint
            // This will eventually be moved to a daily function app.
            if(request == null)
            {
                // Temp safety check for sonar cloud due to the splitting of commands.
                return default;
            }

            await _mediator.Send(new UpdateApprenticeFeedbackTargetCommand { ApprenticeId = request.ApprenticeId });

            // Upon successful update, will now retrieve the valid targets for this sign in Guid
            var result = await _mediator.Send(request);
            _logger.LogDebug($"End Get Apprentice Training Providers for ApprenticeId: {request?.ApprenticeId}");
            return result;
        }


    }
}
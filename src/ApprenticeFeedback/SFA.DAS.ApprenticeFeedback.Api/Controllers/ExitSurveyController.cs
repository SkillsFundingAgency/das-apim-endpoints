using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateExitSurvey;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ExitSurveyController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExitSurveyController> _logger;

        public ExitSurveyController(IMediator mediator, ILogger<ExitSurveyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CreateExitSurveyResponse>> CreateExitSurvey(CreateExitSurveyCommand request)
            => await _mediator.Send(request);
    }
}

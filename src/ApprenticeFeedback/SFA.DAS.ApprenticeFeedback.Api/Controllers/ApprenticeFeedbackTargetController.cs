﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateExitSurvey;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticeFeedbackTargetController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeFeedbackTargetController> _logger;

        public ApprenticeFeedbackTargetController(IMediator mediator, ILogger<ApprenticeFeedbackTargetController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CreateApprenticeFeedbackTargetResponse>> CreateFeedbackTarget(CreateApprenticeFeedbackTargetCommand request)
            => await _mediator.Send(request);


        [HttpPost("{id:Guid}/exitsurvey")]
        public async Task<ActionResult<CreateExitSurveyResponse>> CreateExitSurvey(CreateExitSurveyCommand request)
            => await _mediator.Send(request);
    }
}

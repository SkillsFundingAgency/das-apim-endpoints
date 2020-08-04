using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplication;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IMediator mediator, ILogger<ApplicationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPatch]
        [Route("applications")]
        public async Task<IActionResult> ConfirmApplication(ConfirmApplicationRequest request)
        {
            await _mediator.Send(new ConfirmApplicationCommand(request.ApplicationId, request.AccountId, request.DateSubmitted, request.SubmittedBy));

            return new OkResult();
        }
		
		[HttpGet]
        [Route("/accounts/{accountId}/applications/{applicationId}")]
        public async Task<IActionResult> GetApplication(long accountId, Guid applicationId)
        {
            var result = await _mediator.Send(new GetApplicationQuery
            {
                AccountId = accountId,
                ApplicationId = applicationId
            });

            var response = new ApplicationResponse { Application = MapToDto(result.Application) };

            return Ok(response);
        }

        private IncentiveApplicationDto MapToDto(IncentiveApplication application)
        {
            return new IncentiveApplicationDto
            {
                Apprenticeships = application.Apprenticeships.Select(x => MapToDto(x))
            };
        }

        private IncentiveApplicationApprenticeshipDto MapToDto(IncentiveApplicationApprenticeship apprenticeship)
        {
            return new IncentiveApplicationApprenticeshipDto
            {
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                TotalIncentiveAmount = apprenticeship.TotalIncentiveAmount,
                CourseName = apprenticeship.CourseName
            };
        }
    }
}
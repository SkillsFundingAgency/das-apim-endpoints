using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Command.CreateApplication;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("/accounts/{accountId}/applications")]
        public async Task<IActionResult> PostApplication(CreateApplicationRequest request)
        {
            var applicationId = await _mediator.Send(new CreateApplicationCommand(request.ApplicationId, request.AccountId, request.AccountLegalEntityId, request.ApprenticeshipIds));

            return new CreatedResult($"/accounts{request.AccountId}/applications/{request.ApplicationId}", null);
        }

        [HttpGet]
        [Route("/accounts/{accountId}/applications/{applicationId}")]
        public IActionResult GetApplication(long accountId, Guid applicationId)
        {
            return new OkObjectResult(new GetApplicationResponse
            {
                ApplicationId = applicationId,
                AccountId = accountId,
                AccountLegalEntityId = 1000,
                Apprentices =
                    new []
                    {
                        new ApplicationApprenticeshipDto
                        {
                            ApprenticeshipId = 1,
                            Uln = 9876566778,
                            FirstName = "Fred",
                            LastName = "Flintstone",
                            CourseName = "Mining Flint",
                            ExpectedAmount = 1000
                        },
                        new ApplicationApprenticeshipDto
                        {
                            ApprenticeshipId = 2,
                            Uln = 765668998,
                            FirstName = "Barry",
                            LastName = "Rumble",
                            CourseName = "Mining Flint (level 3)",
                            ExpectedAmount = 1000
                        },
                        new ApplicationApprenticeshipDto
                        {
                            ApprenticeshipId = 3,
                            Uln = 998987678,
                            FirstName = "Barry",
                            LastName = "Cryer",
                            CourseName = "Something or other (level 1)",
                            ExpectedAmount = 2500
                        }
                    }
            });
        }
    }
}

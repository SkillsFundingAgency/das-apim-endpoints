using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;

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
        public IActionResult PostApplication(CreateApplicationRequest request)
        {
            return new CreatedResult($"/accounts{request.AccountId}/applications", 
                new CreateApplicationResponse
                {
                    AccountId = request.AccountId, 
                    AccountLegalEntityId = request.AccountLegalEntityId,
                    ApplicationId = request.ApplicationId
                });
        }

        [HttpGet]
        [Route("/accounts/{accountId}/applications/{applicationId}")]
        public IActionResult GetApplication(long accountId, Guid applicationId)
        {
            return new OkObjectResult(new GetApplicationResponse
            {
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

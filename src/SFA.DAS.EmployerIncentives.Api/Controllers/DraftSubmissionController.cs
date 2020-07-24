using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class DraftSubmissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DraftSubmissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("/accounts{accountId}/draft-submissions")]
        public IActionResult PostDraftSubmission(CreateDraftSubmissionRequest request)
        {
            return new CreatedResult($"/accounts{request.AccountId}/draft-submissions", (new CreateDraftSubmissionResponse { DraftSubmissionId = 30001 }));
        }

        [HttpGet]
        [Route("/accounts/{accountId}/draft-submissions/{draftSubmissionId}")]
        public IActionResult GetDraftSubmission(long accountId, long draftSubmissionId)
        {
            if (draftSubmissionId != 30001)
            {
                return NotFound();
            }

            return new OkObjectResult(new GetDraftSubmissionResponse { AccountLegalEntityId = 1000, Apprentices = new DraftSubmissionApprenticeshipDto[]
            {
                new DraftSubmissionApprenticeshipDto
                {
                    ApprenticeshipId = 1,
                    Uln = 9876566778,
                    FirstName = "Fred",
                    LastName = "Flintstone",
                    CourseName = "Mining Flint",
                    ExpectedAmount = 1000
                },
                new DraftSubmissionApprenticeshipDto
                {
                    ApprenticeshipId = 2,
                    Uln = 765668998,
                    FirstName = "Barry",
                    LastName = "Rumble",
                    CourseName = "Mining Flint (level 3)",
                    ExpectedAmount = 1000
                },
                new DraftSubmissionApprenticeshipDto
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

using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Queries;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class EligibleApprenticeshipSearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EligibleApprenticeshipSearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/apprentices")]
        public async Task<IActionResult> GetEligibleApprentices(long accountId, long accountLegalEntityId)
        {
            var result = await _mediator.Send(new GetEligibleApprenticeshipsSearchQuery
            {
                AccountId = accountId, 
                AccountLegalEntityId = accountLegalEntityId
            });

            var apprentices = result.Apprentices.Select(x => (ApprenticeshipDto)x);

            return new OkObjectResult(new EligibleApprenticeshipsResponse { Apprentices = apprentices.ToArray()});
        }
    }
}
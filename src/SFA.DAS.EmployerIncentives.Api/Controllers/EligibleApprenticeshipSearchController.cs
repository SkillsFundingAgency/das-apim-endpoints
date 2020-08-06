using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class EligibleApprenticeshipSearchController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EligibleApprenticeshipSearchController> _logger;

        public EligibleApprenticeshipSearchController(IMediator mediator, ILogger<EligibleApprenticeshipSearchController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("/apprentices")]
        public async Task<IActionResult> GetEligibleApprentices(long accountId, long accountLegalEntityId)
        {
            try
            {
                var result = await _mediator.Send(new GetEligibleApprenticeshipsSearchQuery
                {
                    AccountId = accountId,
                    AccountLegalEntityId = accountLegalEntityId
                });

                var apprentices = result.Apprentices.Select(x=> (EligibleApprenticeshipDto) x);

                return new OkObjectResult(new EligibleApprenticeshipsResponse { Apprentices = apprentices.ToArray()});
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get a GetEligibleApprentices for accountId:{accountId} accountLegalEntityId:{accountLegalEntityId}");
                return BadRequest();
            }
        }
    }
}
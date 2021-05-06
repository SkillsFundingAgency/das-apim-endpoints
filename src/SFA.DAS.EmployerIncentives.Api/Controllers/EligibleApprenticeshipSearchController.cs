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
        [Route("/apprenticeships")]
        public async Task<IActionResult> GetEligibleApprentices(long accountId, long accountLegalEntityId, int pageNumber, int pageSize)
        {
            try
            {
                 var result = await _mediator.Send(new GetEligibleApprenticeshipsSearchQuery
                {
                    AccountId = accountId,
                    AccountLegalEntityId = accountLegalEntityId,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

                var apprentices = result.Apprentices.Where(x => x.ApprenticeshipStatus != InnerApi.Responses.Commitments.ApprenticeshipStatus.Stopped).Select(x=> (EligibleApprenticeshipDto) x);
                var response = new EligibleApprenticesResponse
                {
                    PageNumber = result.PageNumber,
                    PageSize = pageSize,
                    TotalApprenticeships = result.TotalApprenticeships,
                    Apprenticeships = apprentices
                };

                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get a GetEligibleApprentices for accountId:{accountId} accountLegalEntityId:{accountLegalEntityId} pageNumber: {pageNumber} pageSize: {pageSize}");
                return BadRequest();
            }
        }
    }
}
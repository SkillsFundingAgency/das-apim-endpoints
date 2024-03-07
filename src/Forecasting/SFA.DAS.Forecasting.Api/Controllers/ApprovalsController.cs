using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Approvals.Queries.GetAccountIds;
using SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships;

namespace SFA.DAS.Forecasting.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApprovalsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprovalsController> _logger;

        public ApprovalsController(IMediator mediator, ILogger<ApprovalsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("accountIds")]
        public async Task<IActionResult> GetAccountsWithCohorts()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetAccountIdsQuery());
                var result = new GetAccountsWithCohortsResponse { AccountIds = queryResult.AccountIds };
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred getting accounts with cohorts");
                throw;
            }
        }

        [HttpGet]
        [Route("apprenticeships")]
        public async Task<IActionResult> GetApprenticeships(long accountId, string status, int pageNumber, int pageItemCount)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetApprenticeshipsQuery
                {
                    AccountId = accountId,
                    Status = status,
                    PageNumber = pageNumber,
                    PageItemCount = pageItemCount
                });

                var result = (GetApprenticeshipsResponse) queryResult;
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred getting accounts with cohorts");
                throw;
            }
        }
    }
}

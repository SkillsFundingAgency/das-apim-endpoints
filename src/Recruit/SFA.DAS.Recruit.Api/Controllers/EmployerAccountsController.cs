using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAccount;
using SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/{accountId:long}")]
    public class EmployerAccountsController(IMediator mediator, ILogger<EmployerAccountsController> logger)
        : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetById(long accountId)
        {
            try
            {
                var queryResult = await mediator.Send(new GetAccountQuery {AccountId = accountId});

                var returnModel = new GetAccountResponse
                {
                    HashedAccountId = queryResult.HashedAccountId
                };

                return Ok(returnModel);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting account by id");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("legalentities")]
        public async Task<IActionResult> GetAccountLegalEntities(long accountId)
        {
            try
            {
                var queryResult = await mediator.Send(new GetAccountLegalEntitiesQuery { AccountId = accountId });

                return Ok((GetAccountLegalEntitiesResponse)queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting account legal entities for account");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("dashboard")]
        public async Task<IActionResult> GetDashboard([FromRoute] long accountId, [FromQuery] ApplicationStatus status)
        {
            try
            {
                var queryResult = await mediator.Send(new GetDashboardByAccountIdQuery(accountId, status));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting employer dashboard stats");
                return BadRequest();
            }
        }
    }
}
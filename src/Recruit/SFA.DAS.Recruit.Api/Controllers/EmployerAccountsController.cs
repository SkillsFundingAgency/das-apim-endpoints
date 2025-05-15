using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAccount;
using SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByAccountId;
using SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities;
using System.Net;

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
        [Route("getAllLegalEntities")]
        public async Task<IActionResult> GetAllAccountLegalEntities(
            [FromRoute, Required] long accountId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortColumn = "Name",
            [FromQuery] bool isAscending = false, 
            CancellationToken token = default)
        {
            try
            {
                var queryResult = await mediator.Send(new GetAllAccountLegalEntitiesQuery(accountId, pageNumber, pageSize, sortColumn, isAscending), token);

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting all legal entities for account");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("dashboard")]
        public async Task<IActionResult> GetDashboard([FromRoute] long accountId)
        {
            try
            {
                var queryResult = await mediator.Send(new GetDashboardByAccountIdQuery(accountId));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting employer dashboard stats");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("count")]
        public async Task<IActionResult> GetApplicationReviewsCount([FromRoute] long accountId, [FromBody] List<long> vacancyReferences)
        {
            try
            {
                var queryResult = await mediator.Send(new GetApplicationReviewsCountByAccountIdQuery(accountId, vacancyReferences));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting employer application reviews stats");
                return BadRequest();
            }
        }
    }
}
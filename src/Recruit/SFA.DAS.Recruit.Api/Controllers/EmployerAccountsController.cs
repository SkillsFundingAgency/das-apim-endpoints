using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAccount;
using SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByAccountId;
using SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId;
using SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByAccountId;
using SFA.DAS.Recruit.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Application.Queries.GetAlertsByAccountId;
using SFA.DAS.Recruit.Application.Queries.GetVacanciesByAccountId;

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

        [HttpGet]
        [Route("vacancies")]
        public async Task<IActionResult> GetVacancies([FromRoute] long accountId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string sortColumn = "CreatedDate",
            [FromQuery] string sortOrder = "Desc",
            [FromQuery] FilteringOptions filterBy = FilteringOptions.All,
            [FromQuery] string searchTerm = "",
            CancellationToken token = default)
        {
            try
            {
                var queryResult = await mediator.Send(new GetVacanciesByAccountIdQuery(accountId, page, pageSize, sortColumn, sortOrder, filterBy, searchTerm), token);

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting employer vacancies");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("alerts")]
        public async Task<IActionResult> GetEmployerAlerts([FromRoute] long accountId,
            [FromQuery] string userId = null,
            CancellationToken token = default)
        {
            try
            {
                var queryResult = await mediator.Send(new GetAlertsByAccountIdQuery(accountId, userId), token);

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting employer alerts");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("dashboard/vacancies")]
        public async Task<IActionResult> GetDashboardVacanciesCount([FromRoute] long accountId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string sortColumn = "CreatedDate",
            [FromQuery] bool isAscending = false,
            [FromQuery] List<ApplicationReviewStatus> status = null,
            CancellationToken token = default
            )
        {
            try
            {
                var queryResult = await mediator.Send(new GetDashboardVacanciesCountByAccountIdQuery(accountId, pageNumber, pageSize, sortColumn, isAscending, status), token);

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting employer dashboard vacancy count");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("count")]
        public async Task<IActionResult> GetApplicationReviewsCount([FromRoute] long accountId, [FromQuery]string applicationSharedFilteringStatus, [FromBody] List<long> vacancyReferences)
        {
            try
            {
                var queryResult = await mediator.Send(new GetApplicationReviewsCountByAccountIdQuery(accountId, vacancyReferences,applicationSharedFilteringStatus));

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
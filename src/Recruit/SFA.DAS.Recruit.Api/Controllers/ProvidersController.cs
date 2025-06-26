﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByUkprn;
using SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn;
using SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByUkprn;
using SFA.DAS.Recruit.Application.Queries.GetProvider;
using SFA.DAS.Recruit.Application.Queries.GetProviders;
using SFA.DAS.Recruit.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProvidersController(IMediator mediator, ILogger<ProvidersController> logger) : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllProviders()
        {
            try
            {
                var response = await mediator.Send(new GetProvidersQuery());
                var model = new GetProvidersListResponse
                {
                    Providers = response.Providers.Select(c => (GetProviderResponse)c)
                };
                
                return Ok(model);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting all providers");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{ukprn:int}")]
        public async Task<IActionResult> GetProvider(int ukprn)
        {
            try
            {
                var response = await mediator.Send(new GetProviderQuery { UKprn = ukprn });

                if (response?.Provider == null)
                    return NotFound();

                return Ok((GetProviderResponse)response.Provider);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting provider information");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{ukprn:int}/dashboard")]
        public async Task<IActionResult> GetDashboard([FromRoute] int ukprn)
        {
            try
            {
                var queryResult = await mediator.Send(new GetDashboardByUkprnQuery(ukprn));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting employer dashboard stats");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("dashboard/vacancies")]
        public async Task<IActionResult> GetDashboardVacanciesCount([FromRoute] int ukprn,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string sortColumn = "CreatedDate",
            [FromQuery] bool isAscending = false,
            [FromQuery] ApplicationReviewStatus status = ApplicationReviewStatus.New,
            CancellationToken token = default
        )
        {
            try
            {
                var queryResult = await mediator.Send(new GetDashboardVacanciesCountByUkprnQuery(ukprn, pageNumber, pageSize, sortColumn, isAscending, status), token);

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting provider dashboard vacancy count");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("{ukprn:int}/count")]
        public async Task<IActionResult> GetApplicationReviewsCount([FromRoute] int ukprn, [FromBody] List<long> vacancyReferences)
        {
            try
            {
                logger.LogTrace("GetApplicationReviewCount endpoint called for the ukprn : {ukprn}", ukprn);
                logger.LogTrace("GetApplicationReviewCount endpoint called for the payload : {payload}", JsonConvert.SerializeObject(vacancyReferences));

                var queryResult = await mediator.Send(new GetApplicationReviewsCountByUkprnQuery(ukprn, vacancyReferences));

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

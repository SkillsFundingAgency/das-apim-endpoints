using MediatR;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Application.Queries.GetProvidersByLarsCode;
using SFA.DAS.Recruit.Application.Queries.GetVacanciesByUkprn;

namespace SFA.DAS.Recruit.Api.Controllers;

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
    public async Task<IActionResult> GetProvider([FromRoute]int ukprn)
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
    public async Task<IActionResult> GetDashboard([FromRoute] int ukprn,
        [FromQuery][Required] string userId)
    {
        try
        {
            var queryResult = await mediator.Send(new GetDashboardByUkprnQuery(ukprn, userId));

            return Ok(queryResult);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting provider dashboard stats");
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("{ukprn:int}/vacancies")]
    public async Task<IActionResult> GetVacancies([FromRoute] int ukprn,
        [FromQuery] string userId = null,
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
            var queryResult = await mediator.Send(new GetVacanciesByUkprnQuery(ukprn, userId, page, pageSize, sortColumn, sortOrder, filterBy, searchTerm), token);

            return Ok(queryResult);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting provider vacancies");
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("dashboard/{ukprn:int}/vacancies")]
    public async Task<IActionResult> GetDashboardVacanciesCount([FromRoute] int ukprn,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string sortColumn = "CreatedDate",
        [FromQuery] bool isAscending = false,
        [FromQuery] List<ApplicationReviewStatus>? status = null,
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
            logger.LogError(e, "Error getting provider application reviews stats");
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("~/courses/{larsCode:int}/providers")]
    public async Task<IActionResult> GetByLarsCode([FromRoute] int larsCode, CancellationToken token = default)
    {
        var result = await mediator.Send(new GetProvidersByLarsCodeQuery(larsCode), token);
        return Ok(result);
    }
}
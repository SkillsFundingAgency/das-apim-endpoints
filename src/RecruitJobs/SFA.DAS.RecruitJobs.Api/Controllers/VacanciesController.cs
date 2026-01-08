using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RecruitJobs.GraphQL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SFA.DAS.RecruitJobs.Api.Models;
using SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;
using SFA.DAS.RecruitJobs.Domain.Vacancy;
using SFA.DAS.RecruitJobs.GraphQL.RecruitInner.Mappers;
using StrawberryShake;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VacanciesController(ILogger<VacanciesController> logger) : ControllerBase
{
    [HttpGet, Route("getVacanciesToClose")]
    [ProducesResponseType(typeof(DataResponse<IEnumerable<VacancyIdentifier>>), StatusCodes.Status200OK)]
    public async Task<IResult> GetVacanciesToClose(
        [FromQuery, Required] DateTime pointInTime,
        [FromServices] IRecruitGqlClient recruitGqlClient,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient
            .GetVacanciesToClose
            .ExecuteAsync(pointInTime, cancellationToken);
            
        if (!response.IsSuccessResult())
        {
            logger.LogError("An error occured at GetVacanciesToClose: {Errors}", response.FormatErrors());
            return TypedResults.Problem(response.ToProblemDetails());
        }

        var data = response
            .Data!
            .Vacancies
            .Select(x => 
                new VacancyIdentifier(x.Id, x.VacancyReference, VacancyStatus.Live, x.ClosingDate!.Value.UtcDateTime));
        return TypedResults.Ok(new DataResponse<IEnumerable<VacancyIdentifier>>(data));
    }
}

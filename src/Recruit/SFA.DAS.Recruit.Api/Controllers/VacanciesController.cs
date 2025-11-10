using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models.Vacancies;
using SFA.DAS.Recruit.Api.Models.Vacancies.Requests;
using SFA.DAS.Recruit.Api.Models.Vacancies.Responses;
using SFA.DAS.Recruit.Application.Queries.GetNextVacancyReference;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using GetNextVacancyReferenceResponse = SFA.DAS.Recruit.Api.Models.Vacancies.Responses.GetNextVacancyReferenceResponse;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class VacanciesController(
    ILogger<VacanciesController> logger,
    IMediator mediator,
    VacancyMapper vacancyMapper,
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient): ControllerBase
{
    [HttpGet, Route("vacancyreference")]
    public async Task<IActionResult> GetNextVacancyReference()
    {
        var result = await mediator.Send(new GetNextVacancyReferenceQuery());
        return Ok(new GetNextVacancyReferenceResponse(result.Value));
    }

    // TODO: Semi proxy for the inner api endpoint - this should go once we have migrated vacancies over to SQL
    [HttpPost, Route("{vacancyId:guid}")]
    public async Task<IActionResult> PostOne([FromRoute] Guid vacancyId, [FromBody] PostVacancyRequest vacancy)
    {
        var response = await recruitApiClient.PutWithResponseCode<PutVacancyResponse>(new PutVacancyRequest(vacancyId, vacancyMapper.ToInnerDto(vacancy)));
        try
        {
            response.EnsureSuccessStatusCode();
            return Ok(vacancyMapper.ToOuterDto(response.Body));
        }
        catch (ApiResponseException ex)
        {
            logger.LogError(ex, "Error updating vacancy");
            return Problem(title: ex.Message, detail: ex.Error);
        }
    }
}
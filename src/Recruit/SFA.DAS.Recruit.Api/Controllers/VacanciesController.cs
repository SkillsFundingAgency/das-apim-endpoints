using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Models.Vacancies;
using SFA.DAS.Recruit.Api.Models.Vacancies.Requests;
using SFA.DAS.Recruit.Api.Models.Vacancies.Responses;
using SFA.DAS.Recruit.Application.Queries.GetNextVacancyReference;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class VacanciesController(IMediator mediator, VacancyMapper vacancyMapper, IRecruitApiClient<RecruitApiConfiguration> recruitApiClient): ControllerBase
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
        var response = await recruitApiClient.PutWithResponseCode<InnerApi.Responses.PutVacancyResponse>(new InnerApi.Requests.PutVacancyRequest(vacancyId, vacancyMapper.ToInnerDto(vacancy)));
        return Ok(vacancyMapper.ToOuterDto(response.Body));
    }
}
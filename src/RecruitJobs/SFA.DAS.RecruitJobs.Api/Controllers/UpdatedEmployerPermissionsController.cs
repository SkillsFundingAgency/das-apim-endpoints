using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitJobs.Api.Models.Requests;
using SFA.DAS.RecruitJobs.Handlers;
using StrawberryShake;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController]
[Route("updated-employer-permissions")]
public class UpdatedEmployerPermissionsController: ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
    [Route("vacancies/transferable")]
    public async Task<IResult> GetVacanciesToTransfer(
        [FromServices] IRecruitGqlClient recruitGqlClient,
        [FromQuery] int ukprn,
        [FromQuery] long accountLegalEntityId,
        CancellationToken cancellationToken)
    {
        var response = await recruitGqlClient.GetProviderTransferableVacancies.ExecuteAsync(ukprn, accountLegalEntityId, cancellationToken);
        response.EnsureNoErrors();
        var results = (response.Data?.Vacancies ?? []).Select(x => x.Id);
        return TypedResults.Ok(results);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("vacancies/{vacancyId:guid}/transfer")]
    public async Task<IResult> TransferVacancy(
        [FromServices] ITransferProviderVacancyToLegalEntityHandler handler,        
        [FromRoute] Guid vacancyId,
        [FromBody] TransferVacancyRequest transferRequest,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(vacancyId, transferRequest.TransferReason, cancellationToken);
        return Results.Ok();
    }
}


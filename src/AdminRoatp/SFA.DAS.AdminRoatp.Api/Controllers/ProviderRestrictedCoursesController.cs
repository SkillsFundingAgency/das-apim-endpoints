using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateProviderRestrictedApprenticeship;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderNotRestrictedApprenticeships;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderRestrictedApprenticeships;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}")]
public class ProviderRestrictedCoursesController(IMediator _mediator, ILogger<ProviderRestrictedCoursesController> _logger) : ControllerBase
{
    [HttpGet("restricted-apprenticeships")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetProviderRestrictedApprenticeshipsResponse))]
    public async Task<IActionResult> GetProviderRestrictedApprenticeships([FromRoute] int ukprn, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Request to get provider restricted apprenticeships for UKPRN {Ukprn}", ukprn);

        GetProviderRestrictedApprenticeshipsQuery query = new() { Ukprn = ukprn };
        GetProviderRestrictedApprenticeshipsResponse result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("not-restricted-apprenticeships")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetProviderNotRestrictedApprenticeshipsResponse))]
    public async Task<IActionResult> GetProviderNotRestrictedApprenticeships([FromRoute] int ukprn, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Request to get provider not restricted apprenticeships for UKPRN {Ukprn}", ukprn);

        GetProviderNotRestrictedApprenticeshipsQuery query = new() { Ukprn = ukprn };
        GetProviderNotRestrictedApprenticeshipsResponse result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("restricted-apprenticeships/{larsCode}/change")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> UpdateProviderRestrictedApprenticeship([FromRoute] int ukprn, [FromRoute] string larsCode, [FromBody] UpdateProviderRestrictedApprenticeshipModel request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Request to update provider restricted apprenticeship for UKPRN {Ukprn}", ukprn);

        UpdateProviderRestrictedApprenticeshipCommand command = new()
        {
            Ukprn = ukprn,
            LarsCode = larsCode,
            UserId = request.UserId,
            UserDisplayName = request.UserDisplayName,
            LastDateStarts = request.LastDateStarts
        };

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderNotRestrictedApprenticeships;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderRestrictedApprenticeships;
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
}

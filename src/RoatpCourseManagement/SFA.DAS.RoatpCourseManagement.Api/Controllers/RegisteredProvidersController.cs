using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.RegisteredProviders.Queries;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.RegisteredProvider;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Route("")]
public class RegisteredProvidersController : ControllerBase
{
    private readonly ILogger<RegisteredProvidersController> _logger;
    private readonly IMediator _mediator;

    public RegisteredProvidersController(ILogger<RegisteredProvidersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("lookup/registered-providers")]
    [ProducesResponseType(typeof(List<RegisteredProviderModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetRegisteredProviders()
    {
        _logger.LogInformation("Request received for all registered providers from roatp-service");
        var response = await _mediator.Send(new GetRegisteredProvidersQuery());

        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogWarning("Registered providers not gathered, status code {StatusCode}, Error content:[{ErrorContent}]", response.StatusCode, response.ErrorContent);
            return StatusCode((int)response.StatusCode, response.ErrorContent);
        }
        List<RegisteredProviderModel> result = response.Body.Organisations;
        _logger.LogInformation("Found {OrganisationsCount} registered providers", result.Count);
        return Ok(result);
    }
}

using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Contacts.Queries.GetProviderContact;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}/contact")]
public class ProviderContactsController : ControllerBase
{
    private readonly ILogger<ProviderContactsController> _logger;
    private readonly IMediator _mediator;

    public ProviderContactsController(IMediator mediator, ILogger<ProviderContactsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetProviderContact([FromRoute] int ukprn)
    {
        var response = await _mediator.Send(new GetContactQuery(ukprn));

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                _logger.LogInformation("Provider contact not found for valid ukprn {Ukprn}", ukprn);
                return NotFound();
            case HttpStatusCode.BadRequest:
                _logger.LogWarning("Provider contact for ukprn {Ukprn} is bad request", ukprn);
                return BadRequest();
            default:
                _logger.LogInformation("Provider contact details returned for ukprn {Ukprn}", ukprn);
                return Ok(response.Body);
        }
    }
}

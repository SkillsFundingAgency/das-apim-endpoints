using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Contacts.Commands;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;


[ApiController]
[Route("providers/{ukprn}/contact")]
public class ProviderContactCreateController(IMediator _mediator, ILogger<ProviderContactCreateController> _logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProviderContact([FromRoute] int ukprn, [FromBody] CreateProviderContactCommand command)
    {
        _logger.LogInformation("Outer API: Request received to create provider contact for ukprn: {Ukprn} by user: {UserId}", ukprn, command.UserId);
        command.Ukprn = ukprn;
        return new StatusCodeResult(await _mediator.Send(command));
    }
}
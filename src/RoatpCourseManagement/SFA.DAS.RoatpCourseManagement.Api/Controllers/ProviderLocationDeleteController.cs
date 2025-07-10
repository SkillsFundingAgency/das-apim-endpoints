using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.DeleteProviderLocation;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}/locations/{id}")]
public class ProviderLocationDeleteController(ILogger<ProviderLocationDeleteController> logger, IMediator mediator) : ControllerBase
{
    [HttpDelete]
    public async Task<IActionResult> DeleteProviderLocation([FromRoute] int ukprn, [FromRoute] Guid id, DeleteProviderLocationCommand command)
    {
        logger.LogInformation("Outer API: Request received to delete provider location for ukprn: {ukprn} id: {id} by UserId: {userId}", ukprn, id, command.UserId);
        command.Ukprn = ukprn;
        command.Id = id;
        var response = await mediator.Send(command);

        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => NotFound(),
            HttpStatusCode.NoContent => NoContent(),
            _ => BadRequest()
        };
    }
}

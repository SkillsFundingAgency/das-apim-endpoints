using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.DeleteProviderLocation;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Locations")]
[Route("providers/{ukprn}/locations/{id}")]
public class ProviderLocationDeleteController(ILogger<ProviderLocationDeleteController> logger, IMediator mediator) : ControllerBase
{
    [HttpDelete]
    public async Task<IActionResult> DeleteProviderLocation([FromRoute] int ukprn, [FromRoute] Guid id, [FromQuery] Guid userId, [FromQuery] string userDisplayName)
    {

        DeleteProviderLocationCommand command = new DeleteProviderLocationCommand
        { Ukprn = ukprn, Id = id, UserId = userId.ToString(), UserDisplayName = userDisplayName };


        logger.LogInformation("Outer API: Request received to delete provider location for ukprn: {Ukprn} id: {Id} by UserId: {UserId}", ukprn, id, command.UserId);
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

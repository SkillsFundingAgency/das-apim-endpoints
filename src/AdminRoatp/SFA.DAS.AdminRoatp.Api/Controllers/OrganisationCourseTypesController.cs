using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[Route("organisations")]
[ApiController]
public class OrganisationCourseTypesController(IMediator mediator, ILogger<OrganisationCourseTypesController> _logger) : ControllerBase
{
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    [Route("{ukprn}/course-types")]

    public async Task<IActionResult> UpdateCourseTypes([FromRoute] int ukprn, [FromBody] UpdateOrganisationCourseTypeModel model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to update course types for Ukprn: {Ukprn}", ukprn);
        UpdateOrganisationCourseTypesCommand command = new(ukprn, model.CourseTypeIds, model.UserId, model.UserDisplayName);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[Route("organisations")]
[ApiController]
public class OrganisationCourseTypesController(IMediator mediator, ILogger<OrganisationCourseTypesController> _logger) : ControllerBase
{
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    [Route("{ukprn}/course-types")]

    public async Task<IActionResult> UpdateCourseTypes([FromRoute] int ukprn, [FromBody] UpdateCourseTypesModel model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to update course types for Ukprn: {Ukprn}", ukprn);
        UpdateOrganisationCourseTypesCommand command = new(ukprn, model.CourseTypeIds, model.UserId);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

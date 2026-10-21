using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpOversight.Application.Commands.AssignCourseTypesToProvider;

namespace SFA.DAS.RoatpOversight.Api.Controllers;

[Route("providers/course-types")]
[ApiController]
public class ProviderCourseTypesController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddCourseTypes([FromBody] AssignCourseTypesToProviderCommand model, CancellationToken cancellationToken)
    {
        await _mediator.Send(model, cancellationToken);
        return Ok();
    }
}

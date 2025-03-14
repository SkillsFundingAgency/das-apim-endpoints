using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpCourseManagement.Application.Shortlists.Commands.DeleteExpiredShortlists;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShortlistsController(IMediator _mediator) : ControllerBase
{
    [HttpDelete]
    public async Task<IActionResult> DeleteExpiredShortlist()
    {
        await _mediator.Send(new DeleteExpiredShortlistsCommand());
        return Accepted();
    }
}

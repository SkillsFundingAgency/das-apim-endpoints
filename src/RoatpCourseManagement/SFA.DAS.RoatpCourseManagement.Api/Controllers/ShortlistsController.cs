using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpCourseManagement.Application.Shortlists.Commands.DeleteExpiredShortlists;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ShortlistsController(IMediator _mediator) : ControllerBase
{
    [HttpDelete]
    [Route("expired")]
    public async Task<IActionResult> DeleteExpiredShortlist()
    {
        await _mediator.Send(new DeleteExpiredShortlistsCommand());
        return Accepted();
    }
}

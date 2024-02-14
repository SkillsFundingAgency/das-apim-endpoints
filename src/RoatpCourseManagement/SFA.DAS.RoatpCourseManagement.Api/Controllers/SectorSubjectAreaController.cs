using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpCourseManagement.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
public class SectorSubjectAreaController : ControllerBase
{
    private readonly IMediator _mediator;

    public SectorSubjectAreaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("lookup/sector-subject-area-tier1")]
    [HttpGet]
    public async Task<IActionResult> GetAllSectorSubjectAreaTier1()
    {
        var result = await _mediator.Send(new GetAllSectorSubjectAreaTier1Query());

        if (result.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)result.StatusCode, result.ErrorContent);
        }

        return Ok(result.Body);
    }
}

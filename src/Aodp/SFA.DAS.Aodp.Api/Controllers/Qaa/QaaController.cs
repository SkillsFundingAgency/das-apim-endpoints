using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.Qaa;

namespace SFA.DAS.Aodp.Api.Controllers.Qaa;

[ApiController]
[Route("api/[controller]")]
public class QaaController : BaseController
{
    public QaaController(IMediator mediator, ILogger<QaaController> logger) : base(mediator, logger)
    {
    }

    [HttpGet("/api/qaa/download-summary")]
    [ProducesResponseType(typeof(GetQaaDownloadSummaryQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDownloadSummary()
    {
        return await SendRequestAsync(new GetQaaDownloadSummaryQuery());
    }

    [HttpGet("/api/qaa/download")]
    [ProducesResponseType(typeof(GetQaaQualificationsExportQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Download([FromQuery] string username)
    {
        return await SendRequestAsync(new GetQaaQualificationsExportQuery
        {
            CurrentUsername = username
        });
    }
}

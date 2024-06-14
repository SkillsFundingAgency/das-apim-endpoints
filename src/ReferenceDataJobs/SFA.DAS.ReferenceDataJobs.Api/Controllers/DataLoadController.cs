using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ReferenceDataJobs.Application.Commands;

namespace SFA.DAS.ReferenceDataJobs.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DataLoadController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DataLoadController> _logger;

    public DataLoadController(IMediator mediator, ILogger<DataLoadController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start DataLoads");

        try
        {
            await _mediator.Send(new StartDataLoadsCommand(), cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when starting DataLoads");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
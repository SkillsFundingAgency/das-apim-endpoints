using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;
using SFA.DAS.AODP.Application.Commands.OutputFile;
using SFA.DAS.AODP.Application.Queries.OutputFile;

namespace SFA.DAS.Aodp.Api.Controllers.OutputFile;

[ApiController]
[Route("api/[controller]")]
public class OutputFileController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<OutputFileController> _logger;

    public OutputFileController(IMediator mediator, ILogger<OutputFileController> logger) : base(mediator, logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("/api/outputfiles")]
    [ProducesResponseType(typeof(GetPreviousOutputFilesQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = new GetPreviousOutputFilesQuery();
        return await SendRequestAsync(query);
    }

    [HttpPost("/api/outputfiles/generate")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromBody] GenerateNewOutputFileCommand command)
    {
        return await SendRequestAsync(command);
    }
}

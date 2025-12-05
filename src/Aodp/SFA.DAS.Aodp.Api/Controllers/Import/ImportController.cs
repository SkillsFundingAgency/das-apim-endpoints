using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Import;

namespace SFA.DAS.Aodp.Api.Controllers.Import;

[ApiController]
[Route("api/[controller]")]
public class ImportController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<ImportController> _logger;

    public ImportController(IMediator mediator, ILogger<ImportController> logger) : base(mediator, logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("/api/Import/defunding-list")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(BaseMediatrResponse<ImportDefundingListResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ImportDefundingList([FromForm] ImportDefundingListCommand request)
    {
        var file = request.File;

        try
        {
            var command = new ImportDefundingListCommand
            {
                File = file,
            };

            return await SendRequestAsync(command);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading uploaded file");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to read uploaded file" });
        }
    }

    [HttpPost("/api/Import/pldns")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(BaseMediatrResponse<ImportPldnsCommandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ImportPldns([FromForm] ImportPldnsCommand request)
    {
        //var file = request.File;

        try
        {
            var command = new ImportPldnsCommand
            {
                File = request.File,
            };

            return await SendRequestAsync(command);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading uploaded file");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to read uploaded file" });
        }
    }
}
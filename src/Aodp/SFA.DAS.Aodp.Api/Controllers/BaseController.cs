using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Aodp.Api.Controllers;

public class BaseController : Controller
{
    protected readonly IMediator _mediator;
    protected readonly ILogger _logger;

    public BaseController(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> SendRequestAsync<TResponse>(IRequest<BaseMediatrResponse<TResponse>> request) where TResponse : class, new()
    {
        var response = await _mediator.Send(request);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        _logger.LogError(message: $"Error thrown handling request: {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}

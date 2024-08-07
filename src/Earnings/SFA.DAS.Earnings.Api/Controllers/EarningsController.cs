using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class EarningsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EarningsController> _logger;

    public EarningsController(IMediator mediator, ILogger<EarningsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetEarnings()
    {
        throw new NotImplementedException();
    }
}
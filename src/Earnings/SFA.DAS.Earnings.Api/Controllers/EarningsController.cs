using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("earnings")]
public class EarningsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EarningsController> _logger;

    public EarningsController(IMediator mediator, ILogger<EarningsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

}
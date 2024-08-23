using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountQuery;
using System.Threading.Tasks;
using System;
using SFA.DAS.Approvals.Application.Reservations.Queries;

namespace SFA.DAS.Approvals.Api.Controllers;


[ApiController]
[Route("[controller]/")]
public class ReservationsController : ControllerBase
{
    private readonly ILogger<ReservationsController> _logger;
    private readonly IMediator _mediator;

    public ReservationsController(ILogger<ReservationsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("accounts/{accountId}/status")]
    public async Task<IActionResult> Get(long accountId, [FromQuery] long? transferSenderId)
    {
        try
        {
            var result = await _mediator.Send(new GetAccountReservationsStatusQuery { AccountId = accountId, TransferSenderId = transferSenderId });
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting employer account reservation status");
            return BadRequest();
        }
    }
}
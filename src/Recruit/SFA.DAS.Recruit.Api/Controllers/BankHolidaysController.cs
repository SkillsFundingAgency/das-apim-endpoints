using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Application.Queries.GetBankHolidays;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class BankHolidaysController(IMediator mediator, ILogger<BankHolidaysController> logger) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetBankHolidays()
    {
        try
        {
            var result = await mediator.Send(new GetBankHolidaysQuery());
            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            logger.LogError("Error getting bank holidays");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
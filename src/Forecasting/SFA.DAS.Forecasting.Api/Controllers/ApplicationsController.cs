using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications;

namespace SFA.DAS.Forecasting.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ApplicationsController(IMediator mediator) : Controller
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetApplications(int pledgeId)
    {
        var queryResult = await mediator.Send(new GetApplicationsQuery { PledgeId = pledgeId });
        var result = (GetApplicationsResponse)queryResult;
        
        return Ok(result);
    }
}
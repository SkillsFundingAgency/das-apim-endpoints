using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Models.Providers;
using SFA.DAS.EmployerFinance.Application.Queries.GetProvider;

namespace SFA.DAS.EmployerFinance.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProvidersController(IMediator mediator, ILogger<ProvidersController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProvider(int id)
    {
        try
        {
            var response = await mediator.Send(new GetProviderQuery
            {
                Id = id
            });
            var model = (GetProviderResponse) response;
                
            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error getting provider {id}");
            return BadRequest();
        }
    }
}
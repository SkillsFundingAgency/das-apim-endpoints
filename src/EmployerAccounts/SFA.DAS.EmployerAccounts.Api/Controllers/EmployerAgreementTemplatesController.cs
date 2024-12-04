using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAgreementTemplates;

namespace SFA.DAS.EmployerAccounts.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class EmployerAgreementTemplatesController(IMediator _mediator, ILogger<EmployerAgreementTemplatesController> _logger) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await _mediator.Send(new GetEmployerAgreementTemplatesQuery());

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.EmployerAgreementTemplates);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Getting Employer Agreement Templates");
            return BadRequest();
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;

namespace SFA.DAS.EmployerAccounts.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class VacanciesController(IMediator mediator, ILogger<VacanciesController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}")]
    public async Task<IActionResult> GetVacancyData([FromRoute] long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetEmployerVacanciesQuery
            {
                AccountId = accountId
            });

            if (result == null)
            {
                return NotFound();
            }

            var model = (GetEmployerVacanciesApiResponse)result;

            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting employer vacancies");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
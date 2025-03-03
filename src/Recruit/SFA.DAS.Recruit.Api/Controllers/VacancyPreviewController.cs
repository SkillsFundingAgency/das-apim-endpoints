using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class VacancyPreviewController(IMediator mediator, ILogger<VacancyPreviewController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPreview(int standardId)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyPreviewQuery
            {
                StandardId = standardId
            });

            if (result.Course == null)
            {
                return NotFound();
            }
            
            return Ok((GetVacancyPreviewApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting preview");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
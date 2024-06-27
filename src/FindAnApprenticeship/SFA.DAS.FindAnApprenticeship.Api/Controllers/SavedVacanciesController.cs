using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetSavedVacancies;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("saved-vacancies")]
    public class SavedVacanciesController(IMediator mediator, ILogger<ApplicationsController> logger) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetSavedVacanciesQuery
                {
                    CandidateId = candidateId
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Get Saved Vacancies : An error occurred");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}

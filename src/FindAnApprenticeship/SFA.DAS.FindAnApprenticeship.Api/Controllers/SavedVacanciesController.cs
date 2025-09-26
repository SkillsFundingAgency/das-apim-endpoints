using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.DeleteSavedVacancy;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.SaveVacancy;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetSavedVacancies;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("saved-vacancies/")]
    public class SavedVacanciesController(IMediator mediator, ILogger<ApplicationsController> logger) : Controller
    {
        [HttpGet("")]
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

        [HttpPost]
        [Route("{candidateId:guid}/add")]
        public async Task<IActionResult> AddSavedVacancy([FromRoute] Guid candidateId, [FromBody] SaveVacancyApiRequest request)
        {
            try
            {
                var result = await mediator.Send(new SaveVacancyCommand(candidateId, request.VacancyId));

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Post Saved Vacancy : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{candidateId:guid}/delete")]
        public async Task<IActionResult> DeleteSavedVacancy([FromRoute] Guid candidateId, [FromBody] DeleteSavedVacancyApiRequest request)
        {
            try
            {
                var result = await mediator.Send(new DeleteSavedVacancyCommand(candidateId, request.VacancyId, request.DeleteAllByVacancyReference));

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Post Delete Saved Vacancy : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
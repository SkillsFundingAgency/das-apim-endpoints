using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetVacancyDetails;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VacanciesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VacanciesController> _logger;
        private readonly IMetrics _metrics;

        public VacanciesController(IMediator mediator, ILogger<VacanciesController> logger, IMetrics metrics)
        {
            _mediator = mediator;
            _logger = logger;
            _metrics = metrics;
        }

        [HttpGet]
        [Route("{vacancyReference}")]
        [ProducesResponseType(typeof(GetApprenticeshipVacancyApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchByVacancyReference([FromRoute] string vacancyReference, [FromQuery] Guid? candidateId = null)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeshipVacancyQuery
                { VacancyReference = vacancyReference, CandidateId = candidateId });
                if (result == null) return new StatusCodeResult((int)HttpStatusCode.NotFound);
                return Ok((GetApprenticeshipVacancyApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting vacancy details by reference:{vacancyReference}", vacancyReference);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
            finally
            {
                _metrics.IncreaseVacancyViews(vacancyReference);
            }
        }

        [HttpGet]
        [Route("nhs/{vacancyReference}")]
        [ProducesResponseType(typeof(GetApprenticeshipNhsVacancyApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNhsVacancyByReference([FromRoute] string vacancyReference)
        {
            try
            {
                var result = await _mediator.Send(new GetVacancyDetailsQuery(vacancyReference));
                if (result == null) return new StatusCodeResult((int)HttpStatusCode.NotFound);
                return Ok((GetApprenticeshipNhsVacancyApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Nhs vacancy details by reference:{vacancyReference}", vacancyReference);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{vacancyReference}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Apply([FromRoute] string vacancyReference, [FromBody] PostApplyApiRequest request)
        {
            try
            {
                var result = await _mediator.Send(new ApplyCommand
                { CandidateId = request.CandidateId, VacancyReference = vacancyReference });

                if (result == null) return new StatusCodeResult((int)HttpStatusCode.NotFound);

                // increase the count of vacancy started counter metrics.
                _metrics.IncreaseVacancyStarted(vacancyReference);
                
                return Created(result.ApplicationId.ToString(),(PostApplyApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting vacancy details by reference:{vacancyReference}", vacancyReference);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using System.Net;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies;
using SFA.DAS.FindAnApprenticeship.Api.Telemetry;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Vacancies;

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

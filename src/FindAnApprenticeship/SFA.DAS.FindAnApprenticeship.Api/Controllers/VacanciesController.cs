using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using System.Net;
using System.Threading.Tasks;
using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VacanciesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VacanciesController> _logger;

        public VacanciesController(IMediator mediator, ILogger<VacanciesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{vacancyReference}")]
        [ProducesResponseType(typeof(GetApprenticeshipVacancyApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchByVacancyReference([FromRoute] string vacancyReference)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeshipVacancyQuery { VacancyReference = vacancyReference });
                if (result == null) return new StatusCodeResult((int)HttpStatusCode.NotFound);
                return Ok((GetApprenticeshipVacancyApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting vacancy details by reference:{vacancyReference}", vacancyReference);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

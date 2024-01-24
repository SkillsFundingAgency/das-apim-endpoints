using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("vacancies/{vacancyReference}/[controller]")]
    public class ApplyController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplyController> _logger;

        public ApplyController(IMediator mediator, ILogger<ApplyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string vacancyReference, [FromQuery] Guid candidateId)
        {
            var result = await _mediator.Send(new GetIndexQuery
                { CandidateId = candidateId, VacancyReference = vacancyReference });

            return Ok((GetIndexApiResponse) result);
        }
    }
}

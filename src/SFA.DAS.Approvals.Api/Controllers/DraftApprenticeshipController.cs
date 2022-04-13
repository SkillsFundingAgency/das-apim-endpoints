using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/{cohortId}")]
    public class DraftApprenticeshipController : Controller
    {
        private readonly ILogger<DraftApprenticeshipController> _logger;
        private readonly IMediator _mediator;

        public DraftApprenticeshipController(ILogger<DraftApprenticeshipController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(long cohortId)
        {
            try
            {
                var result = await _mediator.Send(new GetDraftApprenticeshipsQuery (cohortId));
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting cohort {id}", cohortId);
                return BadRequest();
            }
        }
    }
}

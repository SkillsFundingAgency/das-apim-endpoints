using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Payments.Api.Models;
using SFA.DAS.Payments.Application.Learners;

namespace SFA.DAS.Payments.Api.Controllers
{
    [ApiController]
    [Route("ILR")]
    public class IlrController : ControllerBase
    {
        private readonly ILogger<IlrController> _logger;
        private readonly IMediator _mediator;

        public IlrController(ILogger<IlrController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{ukprn}/{academicYear}")]
        public async Task<IActionResult> GetLearnerReferences(string ukprn, short academicYear)
        {
            try
            {
                var result = await _mediator.Send(new GetLearnersQuery(ukprn, academicYear));
                var learnerReferences = result.ToLearnerReferenceResponse();
                return Ok(learnerReferences);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error attempting to get Learner References");
                return BadRequest();
            }
        }
    }
}

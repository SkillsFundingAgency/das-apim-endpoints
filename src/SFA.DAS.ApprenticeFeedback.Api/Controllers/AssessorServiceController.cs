using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetLearner;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AssessorServiceController : Controller
    {
        private readonly IMediator _mediator;

        public AssessorServiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("learner/{id}")]
        public async Task<IActionResult> GetLearner(Guid Id)
        {
            try
            {
                var result = await _mediator.Send(new GetLearnerQuery());

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                //log error
                return BadRequest();
            }
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.ValidateDraftLearnerDetails.Queries;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticeDetailsController : Controller
    {
        private readonly IMediator _mediator;

        public ApprenticeDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("validate")]
        public async Task<IActionResult> Validate(string uln, string firstName, string lastName) //TODO: request object?
        {
            var result = await _mediator.Send(new ValidateDraftLearnerDetailsQuery
            {
                Uln= uln,
                FirstName = firstName,
                LastName = lastName
            });

            return Ok(result);
        }
    }
}
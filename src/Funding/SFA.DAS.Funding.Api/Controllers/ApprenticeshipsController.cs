using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Funding.Api.Models;
using SFA.DAS.Funding.Application.Queries.GetApprenticeships;

namespace SFA.DAS.Funding.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeshipsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/{ukprn}/apprenticeships")]
        public async Task<IActionResult> GetAll(long ukprn)
        {
            var result = await _mediator.Send(new GetApprenticeshipsQuery
            {
                Ukprn = ukprn
            });

            var response = new GetApprenticeshipsResponse(result.Apprenticeships);

            return Ok(response);
        }
    }
}
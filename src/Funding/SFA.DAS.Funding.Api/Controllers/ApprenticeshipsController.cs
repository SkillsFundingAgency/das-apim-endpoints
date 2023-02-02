using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Funding.Api.Models;
using SFA.DAS.Funding.Application.Queries.GetApprenticeships;

namespace SFA.DAS.Funding.Api.Controllers
{
    [ApiController]
    public class ApprenticeshipsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeshipsController> _logger;

        public ApprenticeshipsController(IMediator mediator, ILogger<ApprenticeshipsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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

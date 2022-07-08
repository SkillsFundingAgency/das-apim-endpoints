using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class OverlappingTrainingDateRequestController : Controller
    {
        private readonly IMediator _mediator;
        public OverlappingTrainingDateRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateOverlappingTrainingDate([FromBody] CreateOverlappingTrainingDateRequest request)
        {
            var result = await _mediator.Send(new CreateOverlappingTrainingDateRequestCommand()
            {
                ProviderId = request.ProviderId,
                DraftApprneticeshipId = request.DraftApprenticeshipId,
                UserInfo = request.UserInfo
            });

            return Ok(result);
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody] ValidateDraftApprenticeshipRequest request)
        {
            var result = await _mediator.Send(new ValidateDraftApprenticeshipDetailsCommand
            {
                 DraftApprenticeshipRequest = request
            });

            return Ok(result);
        }
    }
}

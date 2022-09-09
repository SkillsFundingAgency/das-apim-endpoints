using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models.OverlappingTrainingDateRequest;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries;
using System;
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
                DraftApprenticeshipId = request.DraftApprenticeshipId,
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

        [HttpGet]
        [Route("{providerId}/validateUlnOverlap")]
        public async Task<IActionResult> ValidateUlnOverlapOnStartDate(long providerId, string uln, string startDate, string endDate)
        {
            var result = await _mediator.Send(new ValidateUlnOverlapOnStartDateQuery
            {
                ProviderId = providerId,
                Uln = uln,
                StartDate = startDate,
                EndDate = endDate
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("{apprenticeshipId}/getOverlapRequest")]
        public async Task<IActionResult> GetOverlapRequest(long draftApprenticeshipId)
        {
            var result = await _mediator.Send(new GetOverlapRequestQuery
            {
                DraftApprenticeshipId = draftApprenticeshipId
            });

            return Ok(result);
        }
    }
}
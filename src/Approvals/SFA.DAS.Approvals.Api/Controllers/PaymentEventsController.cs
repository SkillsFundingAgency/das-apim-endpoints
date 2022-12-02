using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.PaymentEvents.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class PaymentEventsController : Controller
    {
        private readonly IMediator _mediator;

        public PaymentEventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("submissionEvents")]
        public async Task<IActionResult> GetSubmissionEvents(long sinceEventId = 0, DateTime? sinceTime = null, long ukprn = 0, int page = 1)
        {
            var result = await _mediator.Send(new GetPaymentSubmissionEventsQuery
            {
                SinceEventId = sinceEventId,
                SinceTime = sinceTime,
                Ukprn = ukprn,
                Page = page
            });

            return Ok(result);
        }
    }
}

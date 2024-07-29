using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class KsbProgressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KsbProgressController(IMediator mediator)
            => _mediator = mediator;


        // gets the ksb types
        [HttpGet("/apprentices/{id}/progress/ksbTypes")]
        public async Task<IActionResult> GetKsbTypes(Guid id)
        { 
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();            
            return Ok(result.ApprenticeDetails);
        }

        // gets the ksb statuses
        [HttpGet("/apprentices/{id}/progress/ksbStatuses")]
        public async Task<IActionResult> GetKsbStatuses(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }

        // gets the ksbs
        [HttpGet("/apprentices/{id}/progress/ksbs")]
        public async Task<IActionResult> GetKsbs(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }

        // gets the ksb progress
        [HttpGet("/apprentices/{id}/progress/ksbs/{ksbKey}")]
        public async Task<IActionResult> GetKsb(Guid id, int ksbKey)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }

        // update ksb
        [HttpPut("/apprentices/{id}/progress/ksbs/{ksbKey}")]
        public async Task<IActionResult> UpdateKsb(Guid id, int ksbKey)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }

    }
}

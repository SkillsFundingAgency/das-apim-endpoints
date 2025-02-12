using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.AODP.Api.Controllers.Qualification
{
    [ApiController]
    [Route("api/new-qualifications")]
    public class NewQualificationsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllNewQualifications()
        {
            var result = await _mediator.Send(new GetNewQualificationsQuery());

            if (!result.Success)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{qualificationReference}")]
        public async Task<IActionResult> GetQualificationDetails(string qualificationReference)
        {
            if (string.IsNullOrWhiteSpace(qualificationReference))
            {
                return BadRequest(new { message = "Qualification reference cannot be empty" });
            }

            var result = await _mediator.Send(new GetQualificationDetailsQuery { QualificationReference = qualificationReference });

            if (!result.Success)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}

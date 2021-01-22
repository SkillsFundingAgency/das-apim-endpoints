using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.AddEmployerVendorId;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class LegalEntityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LegalEntityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("legalentities/vendorregistrationform/status")]
        public async Task<IActionResult> RefreshVendorRegistrationFormStatus(DateTime from)
        {
            var nextRunDateTime = await _mediator.Send(new RefreshVendorRegistrationFormCaseStatusCommand(from));
            return new OkObjectResult(nextRunDateTime);
        }

        [HttpPut("legalentities/{hashedLegalEntityId}/employervendorid")]
        public async Task<IActionResult> AddEmployerVendorId(string hashedLegalEntityId)
        {
            await _mediator.Send(new GetAndAddEmployerVendorIdCommand(hashedLegalEntityId));

            return NoContent();
        }
    }
}

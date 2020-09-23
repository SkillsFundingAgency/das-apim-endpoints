using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseDetails;
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

        [HttpPatch("legalentities/{legalEntityId}/vendorregistrationform")]
        public async Task<IActionResult> UpdateVendorRegistrationForm(long legalEntityId, string hashedLegalEntityId)
        {
            await _mediator.Send(new UpdateVendorRegistrationFormCaseDetailsCommand(legalEntityId, hashedLegalEntityId));

            return NoContent();
        }

        [HttpPatch("legalentities/{legalEntityId}/vendorregistrationform/{caseId}")]
        public async Task<IActionResult> UpdateVendorRegistrationFormStatus(long legalEntityId, string caseId)
        {
            await _mediator.Send(new UpdateVendorRegistrationFormCaseStatusCommand(legalEntityId, caseId));

            return NoContent();
        }

        [HttpPatch("legalentities/vendorregistrationform/status")]
        public async Task<IActionResult> RefreshVendorRegistrationFormStatus(DateTime from, DateTime to)
        {
            await _mediator.Send(new RefreshVendorRegistrationFormCaseStatusCommand(from, to));

            return NoContent();
        }
    }
}

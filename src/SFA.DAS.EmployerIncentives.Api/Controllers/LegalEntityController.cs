using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseDetails;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;

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
    }
}

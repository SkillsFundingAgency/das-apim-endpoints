using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseDetails;

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
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.AddEmployerVendorId;

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
        public async Task<IActionResult> RefreshVendorRegistrationFormStatus(DateTime from, DateTime to)
        {
            await _mediator.Send(new RefreshVendorRegistrationFormCaseStatusCommand(from, to));

            return NoContent();
        }

        [HttpPut("legalentities/{hashedLegalEntityId}/employervendorid")]
        public async Task<IActionResult> GetAndAddEmployerVendorId(string hashedLegalEntityId)
        {
            await _mediator.Send(new GetAndAddEmployerVendorIdCommand(hashedLegalEntityId));

            return NoContent();
        }
    }
}

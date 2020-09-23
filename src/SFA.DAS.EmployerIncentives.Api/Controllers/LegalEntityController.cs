using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> RefreshVendorRegistrationFormStatus(DateTime from, DateTime to)
        {
            await _mediator.Send(new RefreshVendorRegistrationFormCaseStatusCommand(from, to));

            return NoContent();
        }
    }
}

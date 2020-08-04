using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch]
        [Route("applications")]
        public async Task<IActionResult> ConfirmApplication(ConfirmApplicationRequest request)
        {
            await _mediator.Send(new ConfirmApplicationCommand(request.ApplicationId, request.AccountId, request.DateSubmitted, request.SubmittedBy));

            return new OkResult();
        }
    }
}

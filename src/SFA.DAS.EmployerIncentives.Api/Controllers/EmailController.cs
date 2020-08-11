using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.SendEmail;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("bank-details-required")]
        public async Task<IActionResult> SendBankDetailsRequiredEmail(SendBankDetailsEmailRequest request)
        {
            await _mediator.Send(new SendBankDetailsRequiredEmailCommand(request.AccountId, request.AccountLegalEntityId, request.EmailAddress, request.AddBankDetailsUrl));

            return new OkResult();
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Onboarding.ConfirmDetails.Queries;

namespace SFA.DAS.EmployerAan.Api.Controllers.Onboarding
{
    [ApiController]
    [Route("accounts/{employerAccountId}/onboarding/confirm-details")]
    public class ConfirmDetailsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(long employerAccountId)
        {
            var result = await mediator.Send(new GetOnboardingConfirmDetailsQuery
                { EmployerAccountId = employerAccountId });

            return Ok(result);
        }
    }
}

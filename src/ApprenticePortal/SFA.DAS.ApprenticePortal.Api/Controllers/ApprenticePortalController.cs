using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeUpdate;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Api.Controllers
{
    [ApiController]
    public class ApprenticePortalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticePortalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("/apprentices/{apprenticeId}")]
        public async Task<IActionResult> UpdateApprentice(Guid apprenticeId, UpdateApprenticeRequest request)
        {
            await _mediator.Send(new ApprenticeUpdateCommand
            {
                ApprenticeId = request.ApprenticeId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth
            });

            return NoContent();
        }
    }
}
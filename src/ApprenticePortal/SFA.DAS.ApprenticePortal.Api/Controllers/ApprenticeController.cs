using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeUpdate;
using SFA.DAS.ApprenticePortal.Application.Queries.ApprenticeAccounts;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Api.Controllers
{
    [ApiController]
    public class ApprenticeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet]
        [Route("/apprentices/{id}")]
        public async Task<IActionResult> GetApprentice(Guid id)
        {
            var queryResult = await _mediator.Send(new GetApprenticeQuery
            {
                ApprenticeId = id
            });

            if (queryResult.Apprentice == null)
                return NotFound();

            return Ok(queryResult.Apprentice);
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

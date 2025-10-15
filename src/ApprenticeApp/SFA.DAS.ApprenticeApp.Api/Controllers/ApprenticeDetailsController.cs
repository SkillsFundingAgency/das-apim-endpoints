using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeCommitments;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipRegistration;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Telemetry;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class ApprenticeDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IApprenticeAppMetrics _apprenticeAppMetrics;
        public ApprenticeDetailsController(IMediator mediator, IApprenticeAppMetrics metrics)
        {
            _mediator = mediator;
            _apprenticeAppMetrics = metrics;
        }

        [HttpGet("/apprentices/{id}/details")]
        public async Task<IActionResult> GetApprenticeDetails(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            _apprenticeAppMetrics.IncreaseAccountViews();
            return Ok(result.ApprenticeDetails);
        }

        [HttpGet("/apprentices/{id}/apprenticeships")]
        public async Task<IActionResult> GetApprenticeApprenticeshipRegistration(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeApprenticeshipsQuery { ApprenticeId = id });
            if (result.Apprenticeships == null)
            {
                return NotFound();
            }
            var apprenticeship = result.Apprenticeships.FirstOrDefault(a => a.ConfirmedOn == null);
            if (apprenticeship == null)
            {
                return NotFound();
            }
            var registrationResult = await _mediator.Send(new GetApprenticeshipRegistrationQuery { ApprenticeshipId = apprenticeship.CommitmentsApprenticeshipId });
            if (registrationResult == null)
            {
                return NotFound();
            }
            return Ok(registrationResult.RegistrationId);
        }

        [HttpGet("/apprentices/{email}/registration")]
        public async Task<IActionResult> GetApprenticeshipRegistration(string email)
        {
            var result = await _mediator.Send(new GetApprenticeshipRegistrationByEmailQuery { Email = email });
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result.RegistrationId);
        }
    }
}
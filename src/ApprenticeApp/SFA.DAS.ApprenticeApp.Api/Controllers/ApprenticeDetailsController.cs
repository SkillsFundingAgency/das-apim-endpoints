using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Application.Queries.GetMyApprenticeshipByUln;
using SFA.DAS.ApprenticeApp.Telemetry;
using System;
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

        [HttpGet("/apprentice/{uln}")]
        public async Task<IActionResult> GetApprenticeshipByUln(long uln)
        {
            var result = await _mediator.Send(new GetMyApprenticeshipByUlnQuery { Uln = uln });
            if (result.MyApprenticeship == null) return NotFound();

            return Ok(result.MyApprenticeship);
        }
    }
}
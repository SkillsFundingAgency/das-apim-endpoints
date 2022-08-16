using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Application.Commands;

namespace SFA.DAS.TrackProgress.Controllers;

[ApiController]
public class TrackProgressController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrackProgressController(IMediator mediator) => _mediator = mediator;

    [FromHeader(Name = "x-request-context-subscription-name")]
    public string Ukprn { get; set; } = null!;

    [HttpPost]
	[Route("/apprenticeships/{uln}/{plannedStartDate}/progress")]
    public async Task Post(
        long uln, DateTime plannedStartDate, TrackApprenticeProgress.Progress request)
    {
        await _mediator.Send(
            new TrackApprenticeProgress.Command(
				Application.Models.Ukprn.Parse(Ukprn), uln, plannedStartDate, request));
    }
}


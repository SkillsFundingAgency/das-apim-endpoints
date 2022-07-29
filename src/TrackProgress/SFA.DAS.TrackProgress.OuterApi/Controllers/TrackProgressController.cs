using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Apis.Commitments.Commands;

namespace SFA.DAS.TrackProgress.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackProgressController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrackProgressController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task Post(TrackApprenticeProgress.Command request)
    {
        await _mediator.Send(request);
    }
}
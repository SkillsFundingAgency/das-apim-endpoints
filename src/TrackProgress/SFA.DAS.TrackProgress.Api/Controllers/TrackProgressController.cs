using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Application.Commands;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Application.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SFA.DAS.TrackProgress.Controllers;

[ApiController]
public class TrackProgressController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TrackProgressController> _logger;

    public TrackProgressController(IMediator mediator, ILogger<TrackProgressController> logger) 
        => (_mediator, _logger) = (mediator, logger);

    [FromHeader(Name = SubscriptionHeaderConstants.ForProviderId)]
    public string Ukprn { get; set; } = null!;

    [FromHeader(Name = SubscriptionHeaderConstants.ForSandboxMode)]
    public string? IsSandbox { get; set; }

    [HttpPost]
	[Route("/apprenticeships/{uln}/{plannedStartDate}/progress")]
    public async Task<IActionResult> AddApprenticeshipProgress(
        [Range(1, double.MaxValue, ErrorMessage = "ULN must be greater than zero.")] long uln,
        DateTime plannedStartDate, ProgressDto progress)
    {
        try
        {
            var response = await _mediator.Send(
                new TrackProgressCommand(
                    ProviderContext.Create(Ukprn, IsSandbox), uln, plannedStartDate, progress));
            return response.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding apprenticeship progress.");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}

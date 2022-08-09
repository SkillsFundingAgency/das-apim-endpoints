using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

    [FromHeader(Name = "x-request-context-subscription-name")]
    public string Ukprn { get; set; } = null!;

    [HttpPost]
	[Route("/apprenticeships/{uln}/{plannedStartDate}/progress")]
    public async Task<IActionResult> AddApprenticeshipProgress(
        [Range(1, double.MaxValue, ErrorMessage = "ULN must be greater than zero.")] long uln,
        DateTime plannedStartDate, ProgressDto progress)
    {
        try
        {
            var response = await _mediator.Send(new TrackProgressCommand(Application.Models.Ukprn.Parse(Ukprn), uln, plannedStartDate, progress));
            return response.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding apprenticeship progress.");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}

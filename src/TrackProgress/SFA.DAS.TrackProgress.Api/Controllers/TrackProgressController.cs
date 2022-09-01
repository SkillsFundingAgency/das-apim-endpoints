using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Application.Commands.TrackProgress;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Application.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SFA.DAS.TrackProgress.Api.Controllers;

[ApiController]
public class TrackProgressController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TrackProgressController> _logger;
    private string _providerId = null!;

    public TrackProgressController(IMediator mediator, ILogger<TrackProgressController> logger) 
        => (_mediator, _logger) = (mediator, logger);

    /// <summary>
    /// The UKPRN of the training provider for this apprenticeship.
    /// </summary>
    [FromHeader(Name = SubscriptionHeaderConstants.ForProviderId)]
    public string Ukprn
    {
        get => _providerId;
        set
        {
            // Incoming string is '`Provider-99999-TrackProgressOuterApi'
            var items = value.Split("-");
            if (items.Length >= 2)
                _providerId = items[1];
            else
                _providerId = "No providerId supplied";
        }
    }

    /// <summary>
    /// IsSandbox - test call identifier
    /// </summary>
    [FromHeader(Name = SubscriptionHeaderConstants.ForSandboxMode)]
    public string? IsSandbox { get; set; }

    /// <summary>
    /// Upload an apprentice's progress
    /// </summary>
    /// <param name="uln">The unique learner number of the apprentice</param>
    /// <param name="plannedStartDate">The start date of the apprenticeship</param>
    /// <returns></returns>
    /// <param name="progress"></param>
    [HttpPost]
	[Route("/apprenticeships/{uln}/{plannedStartDate}/progress")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AddApprenticeshipProgress(
        [Range(1, double.MaxValue, ErrorMessage = "ULN must be greater than zero.")] long uln,
        DateTime plannedStartDate, ProgressDto progress)
    {
        try
        {
            await _mediator.Send(new TrackProgressCommand(
                    ProviderContext.Create(Ukprn, IsSandbox), uln, plannedStartDate, progress));
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
        catch (ApprenticeshipNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidTaxonomyRequestException e)
        {
            return new BadRequestObjectResult(CreateProblemDetailsResponse(e.Message, e.Errors));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding apprenticeship progress for ULN: {uln}, Start date: {plannedStartDate}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    private ProblemDetails CreateProblemDetailsResponse(string title, List<ErrorDetail> errors)
    {
        var details = new ProblemDetails
        {
            Title = title,
            Status = StatusCodes.Status400BadRequest,
        };
        details.Extensions.Add("errors", errors);

        return details;
    }

    // build test - remove
    private ProblemDetails CreateProblemTest(string title, List<ErrorDetail> errors)
    {
        var details = new ProblemDetails
        {
            Title = title,
            Status = StatusCodes.Status400BadRequest,
        };
        details.Extensions.Add("errors", errors);

        return details;
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Application.Models;
using SFA.DAS.TrackProgress.Application.Services;
using System.Net;

namespace SFA.DAS.TrackProgress.Application.Commands;

public record TrackProgressCommand(
    Ukprn Ukprn,
    long Uln,
    DateTime PlannedStartDate,
    ProgressDto? Progress) : IRequest<TrackProgressResponse>;

public record ErrorDetail(
    string error,
    string description
    );

public class TrackProgressResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ErrorTitle { get; set; } = string.Empty;
    public List<ErrorDetail> ErrorDetails { get; set; } = new List<ErrorDetail>();

    public IActionResult Result
        => StatusCode switch
        {
            HttpStatusCode.Created => new CreatedResult(string.Empty, null),
            HttpStatusCode.NotFound => new NotFoundObjectResult(Message),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(GetProblemDetails()),
            _ => new ObjectResult(new { StatusCode, Message }),
        };

    private ProblemDetails GetProblemDetails()
    {
        var details = new ProblemDetails
        {
            Title = ErrorTitle,
            Status = (int)StatusCode,
        };
        details.Extensions.Add("errors", ErrorDetails);

        return details;
    }

    public TrackProgressResponse(HttpStatusCode statusCode)
        => StatusCode = statusCode;

    public TrackProgressResponse(HttpStatusCode statusCode, string message)
        => (StatusCode, Message) = (statusCode, message);

    public TrackProgressResponse(HttpStatusCode statusCode, string errorTitle, List<ErrorDetail> errorDetails)
        => (StatusCode, ErrorTitle, ErrorDetails) = (statusCode, errorTitle, errorDetails);
}

public class TrackProgressCommandHandler : IRequestHandler<TrackProgressCommand, TrackProgressResponse>
{
    private readonly CommitmentsV2Service _commitmentsService;

    public TrackProgressCommandHandler(CommitmentsV2Service commitmentsV2Service)
        => _commitmentsService = commitmentsV2Service;

    public async Task<TrackProgressResponse> Handle(TrackProgressCommand request, CancellationToken cancellationToken)
    {
        var apprenticeshipResult = await _commitmentsService.GetApprenticeship(request.Ukprn.Value, request.Uln, request.PlannedStartDate);
        if (apprenticeshipResult.StatusCode != HttpStatusCode.OK)
            return new TrackProgressResponse(apprenticeshipResult.StatusCode, apprenticeshipResult.ErrorContent);

        if (apprenticeshipResult.Body.TotalApprenticeshipsFound == 0)
            return new TrackProgressResponse(HttpStatusCode.NotFound, "Apprenticeship not found");

        if (apprenticeshipResult.Body.TotalApprenticeshipsFound > 1)
            apprenticeshipResult.Body.Apprenticeships?.RemoveAll(x => x.StartDate == x.StopDate);

        if (apprenticeshipResult.Body.Apprenticeships?.Count == 0)
            return new TrackProgressResponse(HttpStatusCode.NotFound, "Apprenticeship not found");

        if (apprenticeshipResult.Body.Apprenticeships?.Count > 1)
            return new TrackProgressResponse(HttpStatusCode.BadRequest, "Multiple apprenticeship records exist", new List<ErrorDetail>());

        var apprenticeship = apprenticeshipResult.Body.Apprenticeships?.FirstOrDefault();

        return new TrackProgressResponse(HttpStatusCode.Created);
    }
}
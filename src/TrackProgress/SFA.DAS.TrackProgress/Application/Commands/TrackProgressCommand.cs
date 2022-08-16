using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Application.Models;
using SFA.DAS.TrackProgress.Application.Services;
using System.Net;

namespace SFA.DAS.TrackProgress.Application.Commands;

public record TrackProgressCommand(
    ProviderContext ProviderContext,
    long Uln,
    DateTime PlannedStartDate,
    ProgressDto? Progress) : IRequest<TrackProgressResponse>;

public class TrackProgressResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;

    public IActionResult Result
        => StatusCode switch
        {
            HttpStatusCode.Created => new CreatedResult(string.Empty, null),
            HttpStatusCode.NotFound => new NotFoundObjectResult(Message),
            _ => new ObjectResult(new { StatusCode, Message }),
        };

    public TrackProgressResponse(HttpStatusCode statusCode)
        => StatusCode = statusCode;

    public TrackProgressResponse(HttpStatusCode statusCode, string message)
        => (StatusCode, Message) = (statusCode, message);
}

public class TrackProgressCommandHandler : IRequestHandler<TrackProgressCommand, TrackProgressResponse>
{
    private readonly CommitmentsV2Service _commitmentsService;

    public TrackProgressCommandHandler(CommitmentsV2Service commitmentsV2Service)
        => _commitmentsService = commitmentsV2Service;

    public async Task<TrackProgressResponse> Handle(TrackProgressCommand request, CancellationToken cancellationToken)
    {
        var apprenticeshipResult = await _commitmentsService.GetApprenticeship(request.ProviderContext.ProviderId, request.Uln, request.PlannedStartDate);
        if (apprenticeshipResult.StatusCode != HttpStatusCode.OK)
            return new TrackProgressResponse(apprenticeshipResult.StatusCode, apprenticeshipResult.ErrorContent);

        if (apprenticeshipResult.Body.TotalApprenticeshipsFound == 0)
        {
            var providerResult = await _commitmentsService.GetProvider(request.ProviderContext.ProviderId);

            if (providerResult.StatusCode == HttpStatusCode.NotFound)
                return new TrackProgressResponse(HttpStatusCode.NotFound, "Provider not found");
            else
                return new TrackProgressResponse(HttpStatusCode.NotFound, "Apprenticeship not found");
        }

        if (apprenticeshipResult.Body.Apprenticeships?.Count(x => x.StartDate == request.PlannedStartDate) > 1)
            return new TrackProgressResponse(HttpStatusCode.NotFound, "Multiple results for start date");

        if (request.ProviderContext.InSandboxMode)
        {
            // Any calls to add the progress record should be avoided when in Sandbox Mode
            return new TrackProgressResponse(HttpStatusCode.Created);
        }

        return new TrackProgressResponse(HttpStatusCode.Created);

    }
}
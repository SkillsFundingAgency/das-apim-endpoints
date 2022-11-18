using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.TrackProgressInternal.Apis.CoursesInnerApi;
using SFA.DAS.TrackProgressInternal.Apis.TrackProgressInnerApi;

namespace SFA.DAS.TrackProgressInternal.Application.Commands.TrackProgress;

public record PopulateKsbsCommand(string Standard) : IRequest;

public class PopulateKsbsCommandHandler : IRequestHandler<PopulateKsbsCommand>
{
    private readonly IInternalApiClient<TrackProgressApiConfiguration> _trackProgressApi;
    private readonly CourseApiClient _coursesApi;
    private readonly ILogger<PopulateKsbsCommandHandler> _logger;

    public PopulateKsbsCommandHandler(
        IInternalApiClient<TrackProgressApiConfiguration> trackProgressApi,
        CourseApiClient coursesApi,
        ILogger<PopulateKsbsCommandHandler> logger)
    {
        _trackProgressApi = trackProgressApi;
        _coursesApi = coursesApi;
        _logger = logger;
    }

    public async Task<Unit> Handle(PopulateKsbsCommand request, CancellationToken cancellationToken)
    {
        var standard = await _coursesApi
            .GetWithResponseCode<GetCourseResponse>(new GetCourseRequest(request.Standard));
        standard.EnsureSuccessStatusCode();

        _logger.LogInformation("Caching {Count} KSBs for {Standard}\n{Ksbs}",
            standard.Body.Ksbs.Count, request.Standard, string.Join("\n", standard.Body.Ksbs.Select(x => x.Id)));

        PopulateKsbsRequest.Payload ksbs = new(standard.Body.Ksbs.Select(ToPayloadKsb).ToArray());

        var response = await _trackProgressApi.PostWithResponseCode<object>(
            new PopulateKsbsRequest(ksbs), includeResponse: false);

        return Unit.Value;

        static PopulateKsbsRequest.Ksb ToPayloadKsb(KsbResponse x)
            => new(x.Id, x.Type, x.Description);
    }
}
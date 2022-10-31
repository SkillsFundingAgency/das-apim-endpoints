using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.TrackProgressInternal.Apis.CoursesInnerApi;
using SFA.DAS.TrackProgressInternal.Apis.TrackProgressInnerApi;
using System.Security.Cryptography.X509Certificates;

namespace SFA.DAS.TrackProgressInternal.Application.Commands.TrackProgress;

public record PopulateKsbsCommand(string Standard) : IRequest;

public class PopulateKsbsCommandHandler : IRequestHandler<PopulateKsbsCommand>
{
    private readonly IInternalApiClient<TrackProgressApiConfiguration> _trackProgressApi;
    private readonly CourseApiClient _coursesApi;

    public PopulateKsbsCommandHandler(
        IInternalApiClient<TrackProgressApiConfiguration> trackProgressApi,
        CourseApiClient coursesApi)
    {
        _trackProgressApi = trackProgressApi;
        _coursesApi = coursesApi;
    }

    public async Task<Unit> Handle(PopulateKsbsCommand request, CancellationToken cancellationToken)
    {
        var standard = await _coursesApi
            .GetWithResponseCode<GetCourseResponse>(new GetCourseRequest(request.Standard));
        standard.EnsureSuccessStatusCode();

        PopulateKsbsRequest.Payload ksbs = new(standard.Body.Ksbs.Select(ToPayloadKsb).ToArray());

        var response = await _trackProgressApi.PostWithResponseCode<object>(
            new PopulateKsbsRequest(request.Standard, ksbs));

        return Unit.Value;

        static PopulateKsbsRequest.Ksb ToPayloadKsb(KsbResponse x)
            => new(x.Id, x.Description);
    }
}
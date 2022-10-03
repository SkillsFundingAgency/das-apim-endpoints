using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.TrackProgressInternal.Apis.TrackProgressInnerApi;

namespace SFA.DAS.TrackProgressInternal.Application.Commands.TrackProgress;
public record AggregateProgressCommand(long CommitmentsApprenticeshipId) : IRequest<AggregateProgressResponse>;

public record AggregateProgressResponse;

public class AggregateProgressCommandHandler : IRequestHandler<AggregateProgressCommand, AggregateProgressResponse>
{
    public AggregateProgressCommandHandler(IInternalApiClient<TrackProgressApiConfiguration> trackProgressApi)
    {
        TrackProgressApi = trackProgressApi;
    }

    public IInternalApiClient<TrackProgressApiConfiguration> TrackProgressApi { get; }

    public async Task<AggregateProgressResponse> Handle(AggregateProgressCommand request, CancellationToken cancellationToken)
    {
        var response = await TrackProgressApi.PostWithResponseCode<AggregateProgressResponse>(
            new AggregateProgressRequest(request.CommitmentsApprenticeshipId));

        await Task.CompletedTask;
        return new AggregateProgressResponse();
    }
}
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.TrackProgress.Application.DTOs;

namespace SFA.DAS.TrackProgress.Apis.TrackProgressInnerApi;

public class CreateTrackProgressRequest : IPostApiRequest
{
    private readonly long _apprenticeshipId;

    public CreateTrackProgressRequest(long apprenticeshipId, KsbProgress data)
    {
        _apprenticeshipId = apprenticeshipId;
        Data = data;
    }

    public string PostUrl => $"/apprenticeship/{_apprenticeshipId}";
    public object Data { get; set; }
}

public class KsbProgress
{
    public ProviderApprenticeshipIdentifier ProviderApprenticeshipIdentifier { get; set; } = null!;
    public long? ApprenticeshipContinuationId { get; set; }
    public ProgressDto.Ksb[] Ksbs { get; set; } = null!;
}

public record ProviderApprenticeshipIdentifier(long ProviderId, long Uln, string StartDate);

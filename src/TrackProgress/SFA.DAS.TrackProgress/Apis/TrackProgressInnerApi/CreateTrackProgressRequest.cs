using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.TrackProgress.Application.DTOs;

namespace SFA.DAS.TrackProgress.Apis.TrackProgressInnerApi;

public class CreateTrackProgressRequest : IPostApiRequest
{
    public CreateTrackProgressRequest(KsbProgress data)
    {
        Data = data;
    }

    public string PostUrl => $"/progress";
    public object Data { get; set; }
}

public class KsbProgress
{
    public long ProviderId { get; set; }
    public long Uln { get; set; }
    public string StandardUid { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public long CommitmentsApprenticeshipId { get; set; }
    public long? CommitmentsContinuationId { get; set; }
    public ProgressDto.Ksb[] Ksbs { get; set; } = null!;
}

using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.TrackProgress.Application.DTOs;

namespace SFA.DAS.TrackProgress.Apis.TrackProgressInnerApi;

public class CreateTrackProgressRequest : IPostApiRequest
{
    private readonly long _providerId;
    private readonly long _uln;
    private readonly DateOnly _startDate;

    public CreateTrackProgressRequest(long providerId, long uln, DateOnly startDate, KsbProgress data)
    {
        _providerId = providerId;
        _uln = uln;
        _startDate = startDate;
        Data = data;
    }

    public string PostUrl => $"/apprenticeship/{_providerId}/{_uln}/{_startDate.ToString("yyyy-MM")}/progress";
    public object Data { get; set; }
}

public class KsbProgress
{
    public long ApprovalId { get; set; }
    public long? ApprovalContinuationId { get; set; }
    public ProgressDto.Ksb[] Ksbs { get; set; } = null!;
}
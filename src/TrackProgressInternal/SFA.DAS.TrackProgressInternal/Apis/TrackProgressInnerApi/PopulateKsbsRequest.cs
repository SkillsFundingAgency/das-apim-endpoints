using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.TrackProgressInternal.Apis.TrackProgressInnerApi.PopulateKsbsRequest;

namespace SFA.DAS.TrackProgressInternal.Apis.TrackProgressInnerApi;

public record PopulateKsbsRequest(Payload Ksbs) : IPostApiRequest
{
    public string PostUrl => $"/courses/ksbs";

    public object Data { get => Ksbs; set => throw new Exception(); }

    public record Ksb(Guid Id, string Type, string Description);
    public record Payload(Ksb[] Ksbs);
}
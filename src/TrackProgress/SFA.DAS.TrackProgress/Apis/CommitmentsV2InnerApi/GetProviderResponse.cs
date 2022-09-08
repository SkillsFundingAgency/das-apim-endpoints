using System.Net;

namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

public class GetProviderResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public long ProviderId { get; set; }
    public string Name { get; set; } = string.Empty;
}
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.TrackProgressInternal.Api;

public class TrackProgressConfiguration
{
    public AzureActiveDirectoryConfiguration AzureAd { get; set; } = new();
    public CommitmentsV2ApiConfiguration CommitmentsV2InnerApi { get; set; } = new();
    public CoursesApiConfiguration CoursesApi { get; set; } = new();
    public TrackProgressApiConfiguration TrackProgressApi { get; set; } = new();
}

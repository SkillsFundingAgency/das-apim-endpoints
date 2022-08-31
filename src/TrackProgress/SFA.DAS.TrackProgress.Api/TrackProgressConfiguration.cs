using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.TrackProgress.Api;

/// <summary></summary>
public class TrackProgressConfiguration
{
    /// <summary></summary>
    public AzureActiveDirectoryConfiguration AzureAd { get; set; } = new();
    /// <summary></summary>
    public CommitmentsV2ApiConfiguration CommitmentsV2InnerApi { get; set; } = new();
    /// <summary></summary>
    public CoursesApiConfiguration CoursesApi { get; set; } = new();
    /// <summary></summary>
    public TrackProgressApiConfiguration TrackProgressApi { get; set; } = new();
}

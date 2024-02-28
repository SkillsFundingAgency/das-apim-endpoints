using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgressInternal.Application.Configuration;

public interface IOwnerApiConfiguration : IInternalApiConfiguration
{ }

public class TrackProgressOwnerApiConfiguration : IOwnerApiConfiguration
{
    public TrackProgressOwnerApiConfiguration()
    {

    }
    public string Identifier { get; set; } = "";
    public string Url { get; set; } = "";
}
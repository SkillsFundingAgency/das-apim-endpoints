using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Interfaces
{
    public interface IContentfulApiConfiguration : IApiConfiguration
    {
        string AccessToken { get; set; }
    }
}
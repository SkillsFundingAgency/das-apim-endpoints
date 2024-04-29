using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Services.Interfaces
{
    public interface ILepsNeApiConfiguration : IApiConfiguration
    {
        string Identifier { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scope { get; set; }
    }
}
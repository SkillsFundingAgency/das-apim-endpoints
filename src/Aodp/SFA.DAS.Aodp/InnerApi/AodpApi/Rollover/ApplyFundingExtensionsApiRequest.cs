using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class ApplyFundingExtensionsApiRequest : IPostApiRequest
{
    public string PostUrl => "api/rollover/applyrolloverextension";

    public object Data { get; set; }
}

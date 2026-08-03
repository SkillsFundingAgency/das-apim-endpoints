using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class ValidateRolloverExtensionApiRequest : IPostApiRequest
{
    public string PostUrl => "api/rollover/validaterolloverextension";

    public object Data { get; set; }
}

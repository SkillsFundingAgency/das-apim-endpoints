using SFA.DAS.Aodp.InnerApi;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class ValidateRolloverExtensionApiRequest : IPostMultipartJsonFileApiRequest
{
    public string PostUrl => "api/rollover/validaterolloverextension";

    public object Data { get; set; }
}

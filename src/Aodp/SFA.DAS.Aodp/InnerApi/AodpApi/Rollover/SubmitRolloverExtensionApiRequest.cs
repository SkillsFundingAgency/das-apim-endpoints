using SFA.DAS.Aodp.InnerApi;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class SubmitRolloverExtensionApiRequest : IPostMultipartJsonFileApiRequest
{
    public string PostUrl => "api/rollover/submitrolloverextension";

    public object Data { get; set; }
}

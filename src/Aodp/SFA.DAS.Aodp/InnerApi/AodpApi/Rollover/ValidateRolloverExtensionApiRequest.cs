using SFA.DAS.Aodp.InnerApi;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class ValidateRolloverExtensionApiRequest : IPostMultipartFormDataApiRequest
{
    public string PostUrl => "api/rollover/validaterolloverextension";

    public object Data { get; set; }

    public IEnumerable<KeyValuePair<string, string>> FormData => MultipartFormDataMapper.Map(Data);
}

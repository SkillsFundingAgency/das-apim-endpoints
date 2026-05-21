using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetPayeSchemeAccountByRefRequest(string encodedPayeScheme) : IGetApiRequest
{
    public string EncodedPayeScheme { get; } = Uri.EscapeDataString(encodedPayeScheme);

    public string GetUrl => $"api/accounthistories?payeRef={EncodedPayeScheme}";
}
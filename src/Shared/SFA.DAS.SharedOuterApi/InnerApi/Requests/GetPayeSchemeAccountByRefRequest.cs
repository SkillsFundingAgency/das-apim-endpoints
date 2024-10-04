using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetPayeSchemeAccountByRefRequest : IGetApiRequest
{
    public string EncodedPayeScheme { get; }

    public GetPayeSchemeAccountByRefRequest(string encodedPayeScheme)
    {
        EncodedPayeScheme = Uri.EscapeDataString(encodedPayeScheme);
    }

    public string GetUrl => $"api/accounthistories?payeRef={EncodedPayeScheme}";
}
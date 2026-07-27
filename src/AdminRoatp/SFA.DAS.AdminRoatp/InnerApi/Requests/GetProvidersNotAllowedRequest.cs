using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class GetProvidersNotAllowedRequest : IGetApiRequest
{
    public string GetUrl => $"/courses/{LarsCode}/providers/not-allowed";
    public string LarsCode { get; }
    public GetProvidersNotAllowedRequest(string larsCode)
    {
        LarsCode = larsCode;
    }
}

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class GetAllowedProvidersRequest : IGetApiRequest
{
    public string GetUrl => $"/courses/{LarsCode}/providers/allowed";
    public string LarsCode { get; }
    public GetAllowedProvidersRequest(string larsCode)
    {
        LarsCode = larsCode;
    }
}

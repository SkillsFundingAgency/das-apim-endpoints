using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class GetStandardByLarsCodeRequest : IGetApiRequest
{
    public string GetUrl => $"standards/{LarsCode}";
    public string LarsCode { get; set; }

    public GetStandardByLarsCodeRequest(string larsCode)
    {
        LarsCode = larsCode;
    }
}
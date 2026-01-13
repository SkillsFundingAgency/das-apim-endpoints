using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
public class GetStandardForLarsCodeRequest : IGetApiRequest
{
    public string GetUrl => $"standards/{LarsCode}";
    public string LarsCode { get; }

    public GetStandardForLarsCodeRequest(string larsCode)
    {
        LarsCode = larsCode;
    }
}
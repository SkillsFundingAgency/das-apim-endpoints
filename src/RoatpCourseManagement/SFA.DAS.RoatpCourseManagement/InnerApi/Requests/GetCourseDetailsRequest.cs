using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
public class GetCourseDetailsRequest : IGetApiRequest
{
    public string GetUrl => $"standards/{LarsCode}";
    public string LarsCode { get; }

    public GetCourseDetailsRequest(string larsCode)
    {
        LarsCode = larsCode;
    }
}
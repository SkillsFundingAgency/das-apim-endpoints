using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAan.Application.InnerApi.Standards.Requests;

public class GetStandardQueryRequest : IGetApiRequest
{
    public string StandardUid { get; }

    public GetStandardQueryRequest(string standardUid)
    {
        StandardUid = standardUid;
    }

    public string GetUrl => $"api/courses/Standards/{StandardUid}";
}
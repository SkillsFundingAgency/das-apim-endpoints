using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;

public class GetStandardRequest : IGetApiRequest
{
    public string StandardUid { get; }

    public GetStandardRequest(string standardUid)
    {
        StandardUid = standardUid;
    }

    public string GetUrl => $"api/courses/Standards/{StandardUid}";
}
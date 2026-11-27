using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public record GetReportsRequest : IGetApiRequest
{
    public string GetUrl => "api/reports?ownerType=Qa";
}

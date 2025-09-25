using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;
public record GetQaDashboardApiRequest : IGetApiRequest
{
    public string GetUrl => "api/vacancyreviews/qa/dashboard";
}
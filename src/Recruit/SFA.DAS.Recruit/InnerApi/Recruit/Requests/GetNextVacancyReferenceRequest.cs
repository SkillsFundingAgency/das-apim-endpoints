using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public class GetNextVacancyReferenceRequest : IGetApiRequest
{
    public string GetUrl => "api/vacancyreference";
}
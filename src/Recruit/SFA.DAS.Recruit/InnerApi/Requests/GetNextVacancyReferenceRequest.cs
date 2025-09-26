using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetNextVacancyReferenceRequest : IGetApiRequest
{
    public string GetUrl => "api/vacancyreference";
}
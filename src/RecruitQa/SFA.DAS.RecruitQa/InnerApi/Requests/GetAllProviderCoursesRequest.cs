using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public record GetAllProviderCoursesRequest(int Ukprn) : IGetApiRequest
{
    public string GetUrl => $"api/providers/{Ukprn}/courses";
}
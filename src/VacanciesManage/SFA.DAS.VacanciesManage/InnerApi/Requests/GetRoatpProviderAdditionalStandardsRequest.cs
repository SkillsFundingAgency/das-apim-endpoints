using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests;

public class GetProviderAdditionalStandardsRequest(int providerId) : IGetApiRequest
{
    public string GetUrl => $"api/providers/{providerId}/courses";
}
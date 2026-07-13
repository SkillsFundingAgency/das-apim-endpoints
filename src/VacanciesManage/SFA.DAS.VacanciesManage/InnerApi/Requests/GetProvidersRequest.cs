using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests;

public class GetProvidersRequest(int providerId) : IGetApiRequest
{
    public string GetUrl => $"api/providers/{providerId}";
}
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

public class GetVacancyRequest : IGetApiRequest
{
    private readonly string _vacancyReference;

    public GetVacancyRequest(string vacancyReference)
    {
        _vacancyReference = vacancyReference;
    }

    public string GetUrl => $"/api/vacancies/{_vacancyReference}";
    public string Version => "2.0";
}

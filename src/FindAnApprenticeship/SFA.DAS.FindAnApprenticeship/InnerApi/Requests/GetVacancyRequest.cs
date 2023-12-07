using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

public class GetVacancyRequest : IGetApiRequest
{
    private readonly string _vacancyReference;

    public GetVacancyRequest(string vacancyReference)
    {
        _vacancyReference = vacancyReference;
    }

    public string GetUrl => $"/api/vacancies/{_vacancyReference}";
}

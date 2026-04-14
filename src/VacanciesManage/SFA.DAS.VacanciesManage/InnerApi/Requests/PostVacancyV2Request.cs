using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests;

public class PostVacancyV2Request(PostVacancyV2RequestData postVacancyRequestData) : IPostApiRequest
{
    public string PostUrl => "api/vacancies?ruleset=All";
    public object Data { get; set; } = postVacancyRequestData;
}
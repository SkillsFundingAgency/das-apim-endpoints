using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests;

public class PostVacancyV2Request : IPostApiRequest
{
    public PostVacancyV2Request(PostVacancyV2RequestData postVacancyRequestData)
    {
        Data = postVacancyRequestData;
    }

    public string PostUrl => "api/vacancies?validateOnly=true&ruleset=All";
    public object Data { get; set; }
}
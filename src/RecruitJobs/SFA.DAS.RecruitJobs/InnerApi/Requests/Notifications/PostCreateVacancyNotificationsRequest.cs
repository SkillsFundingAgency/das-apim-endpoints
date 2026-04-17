using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.Notifications;

public class PostCreateVacancyNotificationsRequest(Guid vacancyId): IPostApiRequest
{
    public string PostUrl => $"api/vacancies/{vacancyId}/create-notifications";
    public object Data { get; set; }
}
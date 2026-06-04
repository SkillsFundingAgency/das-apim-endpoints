using System.Collections.Generic;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.RecruitJobs.InnerApi.Requests.Notifications;

public class PostCreateVacancyNotificationsByStatusRequest(Guid vacancyId, VacancyStatus status, Dictionary<string, string>? data = null): IPostApiRequest
{
    public string PostUrl => $"api/vacancies/{vacancyId}/create-notifications/{status}";
    public object Data { get; set; } = data;
}
using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public class PostCreateVacancyNotificationsRequest(Guid vacancyId): IPostApiRequest
{
    public string PostUrl => $"api/vacancies/{vacancyId}/create-notifications";
    public object Data { get; set; }
}
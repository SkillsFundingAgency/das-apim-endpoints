using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public class GetVacancyReviewByIdRequest(Guid id) : IGetApiRequest
{
    public string GetUrl => $"api/vacancyreviews/{id}";
}
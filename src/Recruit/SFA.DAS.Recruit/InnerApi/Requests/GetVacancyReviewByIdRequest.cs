using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetVacancyReviewByIdRequest(Guid id) : IGetApiRequest
{
    public string GetUrl => $"api/vacancyreviews/{id}";
}
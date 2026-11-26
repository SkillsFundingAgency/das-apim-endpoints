using System;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyReviews;

public class GetVacancyReviewByIdRequest(Guid id) : IGetApiRequest
{
    public string GetUrl => $"api/vacancyreviews/{id}";
}
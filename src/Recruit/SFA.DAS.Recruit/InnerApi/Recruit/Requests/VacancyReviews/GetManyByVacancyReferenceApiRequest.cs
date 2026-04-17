using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyReviews;

public sealed record GetManyByVacancyReferenceApiRequest(long VacancyReference, List<ReviewStatus> StatusList) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            var queryParams = StatusList
                .SelectMany(s => new[] { new KeyValuePair<string, string>("status", s.ToString()) });

            return QueryHelpers.AddQueryString(
                $"api/vacancies/{VacancyReference}/reviews",
                queryParams);
        }
    }
}
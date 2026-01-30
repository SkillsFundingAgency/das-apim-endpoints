using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetProviderVacancyApplicationStatsRequest(int ukprn, IEnumerable<long> vacancyReferences) : IGetApiRequest
{
    public string GetUrl
    {
        get
        {
            var baseUrl = $"api/provider/{ukprn}/vacancies/stats";
            var values = vacancyReferences.Select(x => $"{x}").ToArray(); 
            var queryParams = new KeyValuePair<string, StringValues>("vacancyReferences", new StringValues(values));
            return QueryHelpers.AddQueryString(baseUrl, [queryParams]);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFA.DAS.Vacancies.Enums;

namespace SFA.DAS.Vacancies.InnerApi.Responses;

public class GetVacanciesResponse
{
    [JsonPropertyName("total")]
    public long Total { get; set; }

    [JsonPropertyName("totalFound")]
    public long TotalFound { get; set; }

    [JsonPropertyName("apprenticeshipVacancies")]
    public IEnumerable<GetVacancyApiResponse> ApprenticeshipVacancies { get; set; }
}

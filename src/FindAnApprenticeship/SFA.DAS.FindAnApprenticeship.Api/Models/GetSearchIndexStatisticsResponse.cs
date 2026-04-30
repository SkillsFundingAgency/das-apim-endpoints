using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public sealed class GetSearchIndexStatisticsResponse
{
    public DateTime CreatedDate { get; set; }
    public List<VacancySourceStatistic> IndexStatistics { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}

public sealed record VacancySourceStatistic(string Source, long Count);
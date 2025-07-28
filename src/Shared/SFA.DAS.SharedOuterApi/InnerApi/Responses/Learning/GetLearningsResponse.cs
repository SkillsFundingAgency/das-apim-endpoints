using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

public class GetLearningsResponse
{
    public long Ukprn { get; set; }
    public List<Learning> Learnings { get; set; }
}

public class Learning
{
    public Guid Key { get; set; }
    public string Uln { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public List<Episode> Episodes { get; set; }
    public int AgeAtStartOfApprenticeship { get; set; }
    public DateTime? WithdrawnDate { get; set; }
    public DateTime? CompletionDate { get; set; }
}

public class Episode
{
    public Guid Key { get; set; }
    public string TrainingCode { get; set; }
    public DateTime? LastDayOfLearning { get; set; }
    public List<EpisodePrice> Prices { get; set; }
}

public class EpisodePrice
{
    public Guid Key { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? TrainingPrice { get; set; }
    public decimal? EndPointAssessmentPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public int FundingBandMaximum { get; set; }
}

using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class UpdateOnProgrammeApiPutRequest(Guid learningKey, UpdateOnProgrammeRequest data): IPutApiRequest<UpdateOnProgrammeRequest>
{
    public string PutUrl { get; } = $"learning/{learningKey}/on-programme";
    public UpdateOnProgrammeRequest Data { get; set; } = data;
}

public class UpdateOnProgrammeRequest
{
    public Guid ApprenticeshipEpisodeKey { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? PauseDate { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int? FundingBandMaximum { get; set; }
    public bool IncludesFundingBandMaximumUpdate { get; set; }
    public List<PriceItem> Prices { get; set; } = [];
    public List<PeriodInLearningItem> PeriodsInLearning { get; set; } = [];
    public Care Care { get; set; }
}

public class PriceItem
{
    public Guid Key { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TrainingPrice { get; set; }
    public decimal? EndPointAssessmentPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

[DebuggerDisplay("Start={StartDate.ToString(\"yyyy-MM-dd\")}, End={EndDate.ToString(\"yyyy-MM-dd\")}")]
public class PeriodInLearningItem
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime OriginalExpectedEndDate { get; set; }
}

public class Care
{
    public bool HasEHCP { get; set; }
    public bool IsCareLeaver { get; set; }
    public bool CareLeaverEmployerConsentGiven { get; set; }
}
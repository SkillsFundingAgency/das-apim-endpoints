using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

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
    public List<BreakInLearningItem> BreaksInLearning { get; set; } = [];
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

public class BreakInLearningItem
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PriorPeriodExpectedEndDate { get; set; }
}
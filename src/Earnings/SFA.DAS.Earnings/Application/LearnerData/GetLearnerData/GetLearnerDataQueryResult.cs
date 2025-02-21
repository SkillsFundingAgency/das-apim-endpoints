namespace SFA.DAS.Earnings.Application.LearnerData.GetLearnerData;

public class GetLearnerDataQueryResult
{
    public long TotalRecords { get; init; }
    public List<ApprenticeshipResult> Apprenticeships { get; set; } = [];
}

public class ApprenticeshipResult
{
    public long Uln { get; set; }
}
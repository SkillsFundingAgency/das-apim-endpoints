namespace SFA.DAS.Earnings.Application.ApprovedApprenticeships.GetApprovedApprenticeships;

public class GetApprovedApprenticeshipsQueryResult
{
    public long TotalRecords { get; init; }
    public List<ApprenticeshipResult> Apprenticeships { get; set; } = [];
}

public class ApprenticeshipResult
{
    public long Uln { get; set; }
}
using SFA.DAS.ApprenticeAan.Application.Model;

namespace SFA.DAS.ApprenticeAan.Application.LeavingReasons.Queries.GetLeavingReasons;

public class GetLeavingReasonsQueryResult
{
    public List<LeavingCategory> LeavingCategories { get; set; } = new();
}
namespace SFA.DAS.ProviderPR.InnerApi.Requests;

public class GetProviderRelationshipsRequest
{
    public string? SearchTerm { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
    public bool HasRecruitmentPermission { get; set; }
    public bool HasRecruitmentWithReviewPermission { get; set; }
    public bool HasNoRecruitmentPermission { get; set; }
    public bool? HasPendingRequest { get; set; }
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> result = new();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            result.Add(nameof(SearchTerm), SearchTerm.Trim());
        }

        if (HasCreateCohortPermission.HasValue && HasCreateCohortPermission.Value)
        {
            result.Add(nameof(HasCreateCohortPermission), true.ToString());
        }

        if (HasRecruitmentPermission)
        {
            result.Add(nameof(HasRecruitmentPermission), HasRecruitmentPermission.ToString());
        }

        if (HasRecruitmentWithReviewPermission)
        {
            result.Add(nameof(HasRecruitmentWithReviewPermission), HasRecruitmentWithReviewPermission.ToString());
        }

        if (HasNoRecruitmentPermission)
        {
            result.Add(nameof(HasNoRecruitmentPermission), HasNoRecruitmentPermission.ToString());
        }

        if (HasPendingRequest.HasValue && HasPendingRequest.Value)
        {
            result.Add(nameof(HasPendingRequest), true.ToString());
        }

        if (PageSize.HasValue && PageSize.Value > 0)
        {
            result.Add(nameof(PageSize), PageSize.Value.ToString());
        }

        if (PageNumber.HasValue && PageNumber.Value > 0)
        {
            result.Add(nameof(PageNumber), PageNumber.Value.ToString());
        }

        return result;
    }
}

using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetLearnersForProviderRequest
{
    public string Filter { get; set; }
    public string SortColumn { get; set; }
    public bool SortDescending { get; set; }
    public int Page { get; set; }
    public int? PageSize { get; set; }
    public int? StartMonth { get; set; }
    public int StartYear { get; set; }
    public DateTime? MaxStartDate { get; set; }
    public List<long> ExcludeUlns { get; set; }
    public string CourseCode { get; set; }
    public LearningType? LearningType { get; set; }
}
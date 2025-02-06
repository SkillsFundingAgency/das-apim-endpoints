using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public class GetCourseProvidersResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int LarsCode { get; set; }
    public string StandardName { get; set; }
    public string QarPeriod { get; set; }
    public string ReviewPeriod { get; set; }

    public List<ProviderData> Providers { get; set; }
}
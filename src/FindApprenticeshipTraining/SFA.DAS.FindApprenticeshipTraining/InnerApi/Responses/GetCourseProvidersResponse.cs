using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;


public class GetCourseProvidersResponseBase
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public string StandardName { get; set; }
    public CourseType CourseType { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }

    public string QarPeriod { get; set; }
    public string ReviewPeriod { get; set; }

    public List<ProviderData> Providers { get; set; }
}
public class GetCourseProvidersResponseFromCourseApi : GetCourseProvidersResponseBase
{
    public string LarsCode { get; set; }
}

public class GetCourseProvidersResponse : GetCourseProvidersResponseBase
{
    public string LarsCode { get; set; }

    public static implicit operator GetCourseProvidersResponse(GetCourseProvidersResponseFromCourseApi source)
    {
        return new()
        {
            Page = source.Page,
            PageSize = source.PageSize,
            TotalPages = source.TotalPages,
            TotalCount = source.TotalCount,
            StandardName = source.StandardName,
            CourseType = source.CourseType,
            ApprenticeshipType = source.ApprenticeshipType,
            QarPeriod = source.QarPeriod,
            ReviewPeriod = source.ReviewPeriod,
            Providers = source.Providers,
            LarsCode = source.LarsCode
        };
    }
}
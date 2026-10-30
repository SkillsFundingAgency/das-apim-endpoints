using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetAvailableCourseForProviderRequest : IGetApiRequest
{
    private readonly int _ukprn;
    private readonly CourseType _courseType;
    private readonly string _larsCode;
    public GetAvailableCourseForProviderRequest(int ukprn, CourseType courseType, string larsCode)
    {
        _ukprn = ukprn;
        _courseType = courseType;
        _larsCode = larsCode;
    }
    public string GetUrl => $"providers/{_ukprn}/course-types/{_courseType}/available-courses/{_larsCode}";
}

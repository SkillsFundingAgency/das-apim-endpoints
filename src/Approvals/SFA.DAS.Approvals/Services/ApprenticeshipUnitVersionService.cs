using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Services;

public class ApprenticeshipUnitVersionService(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IApprenticeshipUnitVersionService
{
    public async Task<GetCourseLookupResponse> Resolve(string courseCode, DateTime? actualStartDate)
    {
        var course = await coursesApiClient.Get<GetCourseLookupResponse>(new GetCourseLookupByIdRequest(courseCode));
        if (course == null)
        {
            throw new InvalidOperationException($"Course not found for course ID {courseCode}");
        }

        return await Resolve(course, actualStartDate);
    }

    public async Task<GetCourseLookupResponse> Resolve(GetCourseLookupResponse course, DateTime? actualStartDate)
    {
        if (course?.LearningType != "ApprenticeshipUnit")
        {
            return null;
        }

        if (!actualStartDate.HasValue)
        {
            throw new InvalidOperationException("An actual start date is required to resolve an apprenticeship unit version.");
        }

        var response = await coursesApiClient.Get<GetCourseVersionsResponse>(
            new GetCourseVersionsRequest(course.IfateReferenceNumber));
        var startDate = actualStartDate.Value.Date;
        var version = response?.Courses?
            .Where(x => x.VersionDetail != null
                && x.VersionDetail.EarliestStartDate.HasValue
                && x.VersionDetail.EarliestStartDate.Value.Date <= startDate
                && (!x.VersionDetail.LatestStartDate.HasValue || x.VersionDetail.LatestStartDate.Value.Date >= startDate))
            .OrderByDescending(x => x.VersionMajor)
            .ThenByDescending(x => x.VersionMinor)
            .FirstOrDefault();

        if (version == null || string.IsNullOrWhiteSpace(version.Version) || string.IsNullOrWhiteSpace(version.StandardUId))
        {
            throw new InvalidOperationException($"No apprenticeship unit version found for {course.IfateReferenceNumber} on {startDate:yyyy-MM-dd}.");
        }

        return version;
    }
}

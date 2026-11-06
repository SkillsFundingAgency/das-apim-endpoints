using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Services;

public class ApprenticeshipUnitVersionService(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IApprenticeshipUnitVersionService
{
    private readonly Dictionary<string, GetCourseVersionsResponse> _courseVersions = [];

    public async Task<GetCourseLookupResponse> Resolve(string courseCode, DateTime? actualStartDate)
    {
        var course = await coursesApiClient.Get<GetCourseLookupResponse>(new GetCourseLookupByIdRequest(courseCode));
        if (course == null)
        {
            throw new ApprenticeshipUnitVersionException($"Course not found for course ID {courseCode}");
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
            throw new ApprenticeshipUnitVersionException("Enter an actual start date for this apprenticeship unit.");
        }

        if (string.IsNullOrWhiteSpace(course.IfateReferenceNumber))
        {
            throw new ApprenticeshipUnitVersionException("The apprenticeship unit reference number is missing.");
        }

        if (!_courseVersions.TryGetValue(course.IfateReferenceNumber, out var response))
        {
            response = await coursesApiClient.Get<GetCourseVersionsResponse>(
                new GetCourseVersionsRequest(course.IfateReferenceNumber));
            _courseVersions.Add(course.IfateReferenceNumber, response);
        }
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
            throw new ApprenticeshipUnitVersionException("No version of this apprenticeship unit is available for the actual start date.");
        }

        return version;
    }
}

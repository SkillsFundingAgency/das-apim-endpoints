using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;

public record GetAvailableCoursesForProviderQueryResult(IEnumerable<AvailableCourseModel> AvailableCourses);


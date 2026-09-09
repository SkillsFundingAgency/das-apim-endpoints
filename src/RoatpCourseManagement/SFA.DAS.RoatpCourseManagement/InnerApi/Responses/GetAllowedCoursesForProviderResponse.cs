using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

public record GetAllowedCoursesForProviderResponse(IEnumerable<ProviderAllowedCourseModel> AllowedCourses);
public record ProviderAllowedCourseModel(string LarsCode, string Title, int Level);

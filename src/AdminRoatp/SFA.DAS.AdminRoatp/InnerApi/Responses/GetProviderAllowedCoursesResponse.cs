namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public record GetProviderAllowedCoursesResponse(IEnumerable<ProviderAllowedCourseModel> AllowedCourses);
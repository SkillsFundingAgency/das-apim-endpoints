using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourses;

public record GetProviderAllowedCoursesQuery(int Ukprn, CourseType? CourseType) : IRequest<GetProviderAllowedCoursesResponse>;

using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllRestrictedCourses;

public record GetAllRestrictedCoursesQuery(bool Restricted) : IRequest<GetAllRestrictedCoursesResponse>;

using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Queries.GetAllRestrictedCourses;

public record GetAllRestrictedCoursesQuery(bool Restricted) : IRequest<GetAllRestrictedCoursesResponse>;
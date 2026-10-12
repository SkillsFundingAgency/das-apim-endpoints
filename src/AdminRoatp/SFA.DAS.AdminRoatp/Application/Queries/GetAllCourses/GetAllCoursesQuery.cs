using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllCourses;

public record GetAllCoursesQuery : IRequest<GetAllCoursesQueryResult>;

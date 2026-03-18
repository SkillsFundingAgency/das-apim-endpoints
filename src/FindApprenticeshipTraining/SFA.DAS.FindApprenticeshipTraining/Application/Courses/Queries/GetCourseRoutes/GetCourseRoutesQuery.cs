using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseRoutes;

public sealed class GetCourseRoutesQuery : IRequest<GetRoutesListResponse>;
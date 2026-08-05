using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseRoutes;

public sealed class GetCourseRoutesQuery : IRequest<GetRoutesListResponse>;
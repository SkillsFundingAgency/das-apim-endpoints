using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;

public sealed class GetCourseLevelsQuery : IRequest<GetCourseLevelsListResponse>;

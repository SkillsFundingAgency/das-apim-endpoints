using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;

public sealed class GetCourseLevelsQuery : IRequest<GetCourseLevelsListResponse>;

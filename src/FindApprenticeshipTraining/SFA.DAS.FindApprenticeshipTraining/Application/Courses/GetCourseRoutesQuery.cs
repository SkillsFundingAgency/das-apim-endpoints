using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses;

public sealed class GetCourseRoutesQuery : IRequest<GetRoutesListResponse>;
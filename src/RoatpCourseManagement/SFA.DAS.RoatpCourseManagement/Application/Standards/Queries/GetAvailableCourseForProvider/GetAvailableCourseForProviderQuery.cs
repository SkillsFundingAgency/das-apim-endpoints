using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCourseForProvider;

public record GetAvailableCourseForProviderQuery(int Ukprn, CourseType CourseType, string LarsCode) : IRequest<GetAvailableCourseForProviderQueryResult>;

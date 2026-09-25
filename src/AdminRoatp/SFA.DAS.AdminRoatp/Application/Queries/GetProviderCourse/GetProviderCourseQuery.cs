using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderCourse;

public record GetProviderCourseQuery(int Ukprn, string LarsCode) : IRequest<GetProviderCourseResponse?>;

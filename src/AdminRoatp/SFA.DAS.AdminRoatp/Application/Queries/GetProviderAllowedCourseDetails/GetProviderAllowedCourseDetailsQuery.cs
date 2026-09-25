using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourseDetails;

public record GetProviderAllowedCourseDetailsQuery(int Ukprn, string LarsCode) : IRequest<GetProviderAllowedCourseDetailsResponse?>;

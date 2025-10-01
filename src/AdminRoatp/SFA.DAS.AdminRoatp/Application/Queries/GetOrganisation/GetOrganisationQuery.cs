using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
public record GetOrganisationQuery(int ukprn) : IRequest<GetOrganisationResponse?>;
using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
public record GetOrganisationsQuery : IRequest<GetOrganisationsResponse>;
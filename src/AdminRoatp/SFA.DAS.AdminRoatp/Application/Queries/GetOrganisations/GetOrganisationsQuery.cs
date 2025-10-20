using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
public record GetOrganisationsQuery : IRequest<GetOrganisationsQueryResult>;
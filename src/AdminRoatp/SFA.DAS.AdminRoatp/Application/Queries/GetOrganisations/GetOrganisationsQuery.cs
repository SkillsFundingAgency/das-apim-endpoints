using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
public record GetOrganisationsQuery(string SearchTerm) : IRequest<GetOrganisationsQueryResponse>;
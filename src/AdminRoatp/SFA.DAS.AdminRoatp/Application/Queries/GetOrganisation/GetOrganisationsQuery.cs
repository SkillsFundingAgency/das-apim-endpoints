using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
public record GetOrganisationsQuery(string SearchTerm) : IRequest<GetOrganisationsQueryResponse>;
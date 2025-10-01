using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
public record GetOrganisationQuery(int ukprn) : IRequest<GetOrganisationQueryResponse?>;
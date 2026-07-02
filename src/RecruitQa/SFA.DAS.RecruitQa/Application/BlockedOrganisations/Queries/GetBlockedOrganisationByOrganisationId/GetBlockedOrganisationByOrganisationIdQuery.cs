using MediatR;

namespace SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationByOrganisationId;

public sealed record GetBlockedOrganisationByOrganisationIdQuery(string OrganisationId) : IRequest<GetBlockedOrganisationByOrganisationIdQueryResult?>;

using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationByOrganisationId;

public class GetBlockedOrganisationByOrganisationIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetBlockedOrganisationByOrganisationIdQuery, GetBlockedOrganisationByOrganisationIdQueryResult?>
{
    public async Task<GetBlockedOrganisationByOrganisationIdQueryResult?> Handle(GetBlockedOrganisationByOrganisationIdQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<GetBlockedOrganisationResponse>(new GetBlockedOrganisationByOrganisationIdRequest(request.OrganisationId));

        if (response == null)
        {
            return null;
        }

        return new GetBlockedOrganisationByOrganisationIdQueryResult
        {
            BlockedOrganisation = response
        };
    }
}

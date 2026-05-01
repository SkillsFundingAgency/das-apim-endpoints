using MediatR;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationByOrganisationId;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationsByOrganisationType;

public class GetBlockedOrganisationsByOrganisationTypeQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
    : IRequestHandler<GetBlockedOrganisationsByOrganisationTypeQuery, GetGetBlockedOrganisationsByOrganisationTypeQueryResult>
{
    public async Task<GetGetBlockedOrganisationsByOrganisationTypeQueryResult> Handle(GetBlockedOrganisationsByOrganisationTypeQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetAll<GetBlockedOrganisationResponse>(new GetBlockedOrganisationsByOrganisationTypeRequest(request.OrganisationType));

        if (response == null)
        {
            return new GetGetBlockedOrganisationsByOrganisationTypeQueryResult
            {
                BlockedOrganisations = []
            };
        }

        return new GetGetBlockedOrganisationsByOrganisationTypeQueryResult
        {
            BlockedOrganisations = response.ToList()
        };
    }
}
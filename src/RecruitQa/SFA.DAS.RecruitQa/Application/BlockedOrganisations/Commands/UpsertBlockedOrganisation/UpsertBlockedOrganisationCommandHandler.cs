using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.BlockedOrganisations.Commands.UpsertBlockedOrganisation;

public class UpsertBlockedOrganisationCommandHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
    : IRequestHandler<UpsertBlockedOrganisationCommand>
{
    public async Task Handle(UpsertBlockedOrganisationCommand request, CancellationToken cancellationToken)
    {
        await recruitApiClient.Put(new PutBlockedOrganisationRequest(request.Id, new BlockedOrganisationDto
        {
            BlockedStatus = request.BlockedStatus,
            OrganisationId = request.OrganisationId,
            Reason = request.Reason,
            OrganisationType = request.OrganisationType,
            UpdatedByUserEmail = request.UpdatedByUserEmail,
            UpdatedByUserId = request.UpdatedByUserId
        }));
    }
}
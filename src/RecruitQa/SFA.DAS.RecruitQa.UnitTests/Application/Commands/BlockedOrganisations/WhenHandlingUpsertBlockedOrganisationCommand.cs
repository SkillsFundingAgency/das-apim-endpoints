using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Commands.UpsertBlockedOrganisation;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Commands.BlockedOrganisations;

[TestFixture]
internal class WhenHandlingUpsertBlockedOrganisationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Put_Request_Sent(
        UpsertBlockedOrganisationCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] UpsertBlockedOrganisationCommandHandler handler)
    {
        //Act
        await handler.Handle(command, CancellationToken.None);

        //Assert
        recruitApiClient.Verify(x => x.Put(It.Is<PutBlockedOrganisationRequest>(r =>
            r.PutUrl.Contains(command.Id.ToString()) &&
            ((BlockedOrganisationDto)r.Data).OrganisationId == command.OrganisationId &&
            ((BlockedOrganisationDto)r.Data).OrganisationType == command.OrganisationType &&
            ((BlockedOrganisationDto)r.Data).BlockedStatus == command.BlockedStatus &&
            ((BlockedOrganisationDto)r.Data).Reason == command.Reason &&
            ((BlockedOrganisationDto)r.Data).UpdatedByUserId == command.UpdatedByUserId &&
            ((BlockedOrganisationDto)r.Data).UpdatedByUserEmail == command.UpdatedByUserEmail
        )), Times.Once);
    }
}

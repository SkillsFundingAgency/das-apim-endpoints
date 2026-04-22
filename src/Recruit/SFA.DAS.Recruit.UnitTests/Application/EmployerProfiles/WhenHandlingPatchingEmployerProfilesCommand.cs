using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.Application.EmployerProfile.Commands.PatchEmployerProfile;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.EmployerProfiles;

[TestFixture]
internal class WhenHandlingPatchingEmployerProfilesCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_The_Api_Called_And_Success(
        PatchEmployerProfileCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] PatchEmployerProfileCommandHandler handler)
    {
        // arrange
        var expectedPatchRequest = new PatchEmployerProfileApiRequest(command.AccountLegalEntityId, new JsonPatchDocument<Recruit.InnerApi.Models.EmployerProfile>());

        recruitApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchEmployerProfileApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.NoContent, string.Empty));

        // act
        await handler.Handle(command, CancellationToken.None);

        // assert
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchEmployerProfileApiRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Response_NotFound_CommandResult_Is_Returned_As_Expected(
        PatchEmployerProfileCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] PatchEmployerProfileCommandHandler handler)
    {
        var expectedPatchRequest = new PatchEmployerProfileApiRequest(command.AccountLegalEntityId, new JsonPatchDocument<Recruit.InnerApi.Models.EmployerProfile>());

        recruitApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchEmployerProfileApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.NotFound, string.Empty));

        await handler.Handle(command, CancellationToken.None);

        // assert
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchEmployerProfileApiRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Response_IsNotSuccess_CommandResult_Is_Returned_As_Expected(
        PatchEmployerProfileCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] PatchEmployerProfileCommandHandler handler)
    {
        var expectedPatchRequest = new PatchEmployerProfileApiRequest(command.AccountLegalEntityId, new JsonPatchDocument<Recruit.InnerApi.Models.EmployerProfile>());

        recruitApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchEmployerProfileApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.InternalServerError, string.Empty));

        var act = async () => { await handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<ApiResponseException>();
    }
}
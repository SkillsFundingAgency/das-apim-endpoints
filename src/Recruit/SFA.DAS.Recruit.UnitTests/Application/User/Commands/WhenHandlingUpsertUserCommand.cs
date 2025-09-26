using System.Net;
using System.Threading;
using SFA.DAS.Recruit.Application.User.Commands.UpsertUser;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.User.Commands;

public class WhenHandlingUpsertUserCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Called(
        UpsertUserCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        UpsertUserCommandHandler handler)
    {
        var expectedPutRequest = new PutUserRequest(command.Id, command.User);
        recruitApiClient.Setup(
                x => x.PutWithResponseCode<NullResponse>(
                    It.Is<PutUserRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.Created, ""));

        await handler.Handle(command, CancellationToken.None);

        recruitApiClient.Verify(
            x => x.PutWithResponseCode<NullResponse>(
                It.Is<PutUserRequest>(c => c.PutUrl == expectedPutRequest.PutUrl)), Times.Once);
    }
}
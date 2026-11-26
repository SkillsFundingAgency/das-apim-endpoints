using System.Net;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Recruit.Application.User.Commands.UpsertUser;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;

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
    
    [Test, MoqAutoData]
    public async Task Then_An_Error_Is_Thrown_Is_The_Call_Is_Unsuccessful(
        UpsertUserCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        UpsertUserCommandHandler handler)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutUserRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.BadRequest, "Some error text"));

        // act
        var action = async () => await handler.Handle(command, CancellationToken.None);

        // assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
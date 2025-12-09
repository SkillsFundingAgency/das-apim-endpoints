using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Commands.EmployerUsers;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Commands;

public class SuspendEmployerUserCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_Sends_Request_To_EmployerProfiles_Api(
        string identifier,
        string changedByUserId,
        string changedByEmail,
        ChangeUserStatusResponse apiResponse,
        [Frozen] Mock<IInternalApiClient<EmployerProfilesApiConfiguration>> apiClient,
        SuspendEmployerUserCommandHandler handler)
    {
        // Arrange
        var expectedResponse = new ApiResponse<ChangeUserStatusResponse>(apiResponse, HttpStatusCode.OK, string.Empty);

        apiClient.Setup(x =>
                x.PostWithResponseCode<ChangeUserStatusResponse>(It.Is<SuspendEmployerUserRequest>(r =>
                    r.PostUrl == $"api/users/{identifier}/suspend" &&
                    r.Data is ChangeUserStatusRequestData &&
                    ((ChangeUserStatusRequestData)r.Data).ChangedByEmail == changedByEmail &&
                    ((ChangeUserStatusRequestData)r.Data).ChangedByUserId == changedByUserId), true))
            .ReturnsAsync(expectedResponse)
            .Verifiable();

        var command = new SuspendEmployerUserCommand(identifier, changedByUserId, changedByEmail);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResponse);
        apiClient.Verify();
        apiClient.VerifyNoOtherCalls();
    }
}


using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Commands.SupportCreateInvitation;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Commands;

public class SupportCreateInvitationCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_SendInvitation_Returns_HttpStatusCode(
       SupportCreateInvitationCommand command,
       [Frozen] Mock<IAccountsService> accountsService,
       SupportCreateInvitationCommandHandler handler)
    {
        var data = new SendInvitationRequestData
        {
            HashedAccountId = command.HashedAccountId,
            NameOfPersonBeingInvited = command.FullName,
            EmailOfPersonBeingInvited = command.Email,
            RoleOfPersonBeingInvited = command.Role
        };

        SendInvitationRequest request = new(data);

        var response = new ApiResponse<SendInvitationRequest>(request, HttpStatusCode.OK, "");

        accountsService.Setup(x => x.SendInvitation(It.IsAny<SendInvitationRequest>())).ReturnsAsync(response);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(response.StatusCode);
    }
}

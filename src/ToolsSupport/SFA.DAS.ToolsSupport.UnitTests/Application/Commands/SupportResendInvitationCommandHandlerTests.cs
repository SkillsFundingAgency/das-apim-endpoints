using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Commands.SupportResendInvitation;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Commands;

public class SupportResendInvitationCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_ResendInvitationn_Returns_HttpStatusCode(
      SupportResendInvitationCommand command,
      [Frozen] Mock<IAccountsService> accountsService,
      SupportResendInvitationCommandHandler handler)
    {
        var data = new ResendInvitationRequestData
        {
            HashedAccountId = command.HashedAccountId,
            Email = command.Email
        };

        ResendInvitationRequest request = new(data);

        var response = new ApiResponse<ResendInvitationRequest>(request, HttpStatusCode.OK, "");

        accountsService.Setup(x => x.ResendInvitation(It.IsAny<ResendInvitationRequest>())).ReturnsAsync(response);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(response.StatusCode);
    }
}

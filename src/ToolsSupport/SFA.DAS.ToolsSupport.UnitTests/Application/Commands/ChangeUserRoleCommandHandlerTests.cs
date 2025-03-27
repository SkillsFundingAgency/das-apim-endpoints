using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Commands.ChangeUserRole;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Commands;

public class ChangeUserRoleCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_ChangeUserRole_Returns_HttpStatusCode(
         ChangeUserRoleCommand command,
         [Frozen] Mock<IAccountsService> accountsService,
         ChangeUserRoleCommandHandler handler)
    {
        var data = new ChangeUserRoleRequestData
        {
            HashedAccountId = command.HashedAccountId,
            Email = command.Email,
            Role = command.Role
        };

        ChangeUserRoleRequest request = new(data);

        var response = new ApiResponse<ChangeUserRoleRequest>(request, HttpStatusCode.OK, "");

        accountsService.Setup(x => x.ChangeUserRole(It.IsAny<ChangeUserRoleRequest>())).ReturnsAsync(response);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(response.StatusCode);
    }
}

using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries.GetTeamMembers;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;
public class GetTeamMembersQueryHanderTests
{
    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnTeamMembers_WhenTeamMembersExist(
        GetTeamMembersQuery query,
        List<TeamMember> teamMembers,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        GetTeamMembersQueryHander handler)
    {
        // Arrange
        mockAccountsService.Setup(s => s.GetAccountTeamMembers(query.AccountId)).ReturnsAsync(teamMembers);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TeamMembers.Should().NotBeNull();
        result.TeamMembers.Should().BeEquivalentTo(teamMembers);
        mockAccountsService.Verify(s => s.GetAccountTeamMembers(query.AccountId), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnEmptyTeamMembers_WhenTeamMembersAreNull(
        GetTeamMembersQuery query,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        GetTeamMembersQueryHander handler)
    {
        // Arrange
        mockAccountsService.Setup(s => s.GetAccountTeamMembers(query.AccountId)).ReturnsAsync((List<TeamMember>)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TeamMembers.Should().NotBeNull();
        result.TeamMembers.Should().BeEmpty();
        mockAccountsService.Verify(s => s.GetAccountTeamMembers(query.AccountId), Times.Once());
    }
}

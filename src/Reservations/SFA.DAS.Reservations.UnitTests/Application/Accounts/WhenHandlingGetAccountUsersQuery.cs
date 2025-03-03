using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.Accounts.Queries;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.Accounts;

public class WhenHandlingGetAccountUsersQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetAccountUsersQuery query,
        List<TeamMember> teamResponse,
        [Frozen] Mock<IEmployerAccountsService> service,
        GetAccountUsersQueryHandler sut
        )
    {
        service.Setup(x => x.GetTeamMembers(query.AccountId)).ReturnsAsync(teamResponse);

        var actual = await sut.Handle(query, CancellationToken.None);

        service.Verify(x => x.GetTeamMembers(query.AccountId), Times.Once);
        
        actual.TeamMembers.Should().BeEquivalentTo(teamResponse);
    }
}
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.UserAccounts.Queries.GetUserAccounts;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.UserAccounts.Queries.GetUserAccounts;

public class GetUserAccountsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesEmployerAccountService(
        [Frozen] Mock<IEmployerAccountsService> employerAccountServiceMock,
        GetUserAccountsQueryHandler sut,
        GetUserAccountsQuery query,
        IEnumerable<EmployerAccountUser> employerAccountUsers,
        CancellationToken cancellationToken)
    {
        employerAccountServiceMock.Setup(e => e.GetEmployerAccounts(It.Is<EmployerProfile>(p => p.UserId == query.UserId && p.Email == query.Email))).ReturnsAsync(employerAccountUsers);

        await sut.Handle(query, cancellationToken);

        employerAccountServiceMock.Verify(e => e.GetEmployerAccounts(It.Is<EmployerProfile>(p => p.UserId == query.UserId && p.Email == query.Email)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_InvokesEmployerAccountService_ReturnsData(
        [Frozen] Mock<IEmployerAccountsService> employerAccountServiceMock,
        GetUserAccountsQueryHandler sut,
        GetUserAccountsQuery query,
        IEnumerable<EmployerAccountUser> employerAccountUsers,
        CancellationToken cancellationToken)
    {
        employerAccountServiceMock.Setup(e => e.GetEmployerAccounts(It.Is<EmployerProfile>(p => p.UserId == query.UserId && p.Email == query.Email))).ReturnsAsync(employerAccountUsers);

        var expected = employerAccountUsers.First();

        GetUserAccountsQueryResult actual = await sut.Handle(query, cancellationToken);

        using (new AssertionScope())
        {
            actual.EmployerUserId.Should().Be(expected.UserId);
            actual.FirstName.Should().Be(expected.FirstName);
            actual.LastName.Should().Be(expected.LastName);
            actual.IsSuspended.Should().Be(expected.IsSuspended);
            actual.UserAccountResponse.Should().HaveCount(employerAccountUsers.Count());
            actual.UserAccountResponse.Should().BeEquivalentTo(employerAccountUsers, c => c.ExcludingMissingMembers());
        }
    }
}

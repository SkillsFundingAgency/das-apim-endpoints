// MFCMFC
// using AutoFixture.NUnit3;
// using FluentAssertions;
// using Moq;
// using SFA.DAS.EmployerAan.Application.User.GetUserAccounts;
// using SFA.DAS.EmployerAan.Models;
// using SFA.DAS.EmployerAan.Services;
//
//
// namespace SFA.DAS.EmployerAan.UnitTests.Application.User.GetUserAccounts;
//
// public class GetUserAccountsQueryHandlerTests
// {
//     [Test, AutoData]
//     public async Task Handle_NoAccounts_ThrowsException(GetUserAccountsQuery query)
//     {
//         Mock<IEmployerAccountsService> accountServiceMock = new();
//         GetUserAccountsQueryHandler sut = new(accountServiceMock.Object);
//
//         accountServiceMock.Setup(s => s.GetEmployerAccounts(It.Is<EmployerProfile>(p => p.Email == query.Email && p.UserId == query.UserId))).ReturnsAsync(Enumerable.Empty<EmployerAccountUser>());
//
//         Func<Task> action = () => sut.Handle(query, CancellationToken.None);
//
//         await action.Should().ThrowAsync<InvalidOperationException>();
//     }
//
//     [Test, AutoData]
//     public async Task Handle_AccountsFound_ReturnsDetailsFromFirstItem(GetUserAccountsQuery query, IEnumerable<EmployerAccountUser> accounts)
//     {
//         Mock<IEmployerAccountsService> accountServiceMock = new();
//         GetUserAccountsQueryHandler sut = new(accountServiceMock.Object);
//
//         accountServiceMock.Setup(s => s.GetEmployerAccounts(It.Is<EmployerProfile>(p => p.Email == query.Email && p.UserId == query.UserId))).ReturnsAsync(accounts);
//
//         var firstAccount = accounts.First();
//
//         var result = await sut.Handle(query, CancellationToken.None);
//
//         result.FirstName.Should().Be(firstAccount.FirstName);
//         result.LastName.Should().Be(firstAccount.LastName);
//         result.EmployerUserId.Should().Be(firstAccount.UserId);
//         result.IsSuspended.Should().Be(firstAccount.IsSuspended);
//         result.UserAccountResponse.Should().HaveCount(accounts.Count());
//     }
// }

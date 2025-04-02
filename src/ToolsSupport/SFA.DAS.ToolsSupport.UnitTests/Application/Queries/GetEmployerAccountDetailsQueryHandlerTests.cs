using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;

public class GetEmployerAccountDetailsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_GetLatestDetails_from_Reference_Api(
          GetEmployerAccountDetailsQuery query,
          Account account,
          [Frozen] Mock<IAccountsService> accountsService,
          GetEmployerAccountDetailsQueryHandler handler)
    {
        accountsService.Setup(x => x.GetAccount(query.AccountId)).ReturnsAsync(account);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.AccountId.Should().Be(account.AccountId);
        result.HashedAccountId.Should().Be(account.HashedAccountId);
        result.PublicHashedAccountId.Should().Be(account.PublicHashedAccountId);
        result.DasAccountName.Should().Be(account.DasAccountName);
        result.DateRegistered.Should().Be(account.DateRegistered);
        result.OwnerEmail.Should().Be(account.OwnerEmail);
        result.ApprenticeshipEmployerType.Should().Be(account.ApprenticeshipEmployerType);
    }
}

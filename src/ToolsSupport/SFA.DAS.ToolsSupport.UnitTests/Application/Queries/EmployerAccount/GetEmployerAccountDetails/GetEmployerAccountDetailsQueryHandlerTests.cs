using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;
using SFA.DAS.ToolsSupport.Strategies;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries.EmployerAccount.GetEmployerAccountDetails;

public class GetEmployerAccountDetailsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_GetLatestDetails_from_Reference_Api(
          GetEmployerAccountDetailsQuery query,
          GetEmployerAccountDetailsResult expectedResult,
          Account account,
          [Frozen] Mock<IAccountDetailsStrategyFactory> factory,
          [Frozen] Mock<IAccountsService> accountsService,
          Mock<IAccountDetailsStrategy> strategy,
          GetEmployerAccountDetailsQueryHandler handler)
    {
        accountsService.Setup(x => x.GetAccount(query.AccountId)).ReturnsAsync(account);


        factory.Setup(x => x.CreateStrategy(query.SelectedField)).Returns(strategy.Object);

        strategy.Setup(x => x.ExecuteAsync(account)).ReturnsAsync(expectedResult);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }
}

using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;
using SFA.DAS.ToolsSupport.Strategies;

namespace SFA.DAS.ToolsSupport.UnitTests.Strategies;

[TestFixture]
public class AccountDetailsLegalEntitiesStrategyTests()
{
    [Test, MoqAutoData]
    public async Task Then_Execute_Returns_And_IsMapped_To_LegalEntity(
           [Frozen] Mock<IAccountsService> mockApi,
           AccountDetailsLegalEntitiesStrategy strategy,
           Account account,
           LegalEntity legalEntity)
    {
        mockApi
            .Setup(m => m.GetEmployerAccountLegalEntity(It.IsAny<string>()))
            .ReturnsAsync(legalEntity);

        // Act
        var result = await strategy.ExecuteAsync(account);

        result.LegalEntities.Should().NotBeNull();
        result.LegalEntities.Should().HaveCount(account.LegalEntities.Count);
        result.LegalEntities.ToList().First().Should().BeEquivalentTo(legalEntity);
    }
}

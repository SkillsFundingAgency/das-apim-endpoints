using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries.GetAccountOrganisations;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;
public class GetAccountOrganisationsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnLegalEntities_WhenAccountExists(
        GetAccountOrganisationsQuery query,
        Account account,
        List<LegalEntity> legalEntities,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        GetAccountOrganisationsQueryHandler handler)
    {
        // Arrange
        account.LegalEntities =
            [
                new ResourceViewModel { Href = "/legal/1" },
                new ResourceViewModel { Href = "/legal/2" }
            ];
        mockAccountsService.Setup(s => s.GetAccount(query.AccountId)).ReturnsAsync(account);
        mockAccountsService.Setup(s => s.GetEmployerAccountLegalEntity("/legal/1")).ReturnsAsync(legalEntities[0]);
        mockAccountsService.Setup(s => s.GetEmployerAccountLegalEntity("/legal/2")).ReturnsAsync(legalEntities[1]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.LegalEntities.Should().HaveCount(2);
        result.LegalEntities.Should().Contain(legalEntities[0]);
        result.LegalEntities.Should().Contain(legalEntities[1]);
        mockAccountsService.Verify(s => s.GetAccount(query.AccountId), Times.Once());
        mockAccountsService.Verify(s => s.GetEmployerAccountLegalEntity("/legal/1"), Times.Once());
        mockAccountsService.Verify(s => s.GetEmployerAccountLegalEntity("/legal/2"), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnEmptyResult_WhenAccountIsNull(
        GetAccountOrganisationsQuery query,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        GetAccountOrganisationsQueryHandler handler)
    {
        // Arrange
        mockAccountsService.Setup(s => s.GetAccount(query.AccountId)).ReturnsAsync((Account)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.LegalEntities.Should().BeEmpty();
        mockAccountsService.Verify(s => s.GetAccount(query.AccountId), Times.Once());
        mockAccountsService.Verify(s => s.GetEmployerAccountLegalEntity(It.IsAny<string>()), Times.Never());
    }
}
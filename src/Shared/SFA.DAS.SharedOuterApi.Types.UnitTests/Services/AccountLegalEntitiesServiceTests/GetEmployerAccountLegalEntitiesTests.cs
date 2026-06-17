using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Services;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.AccountLegalEntitiesServiceTests;

[TestFixture]
internal class GetEmployerAccountLegalEntitiesTests
{
    [Test, MoqAutoData]
    public async Task GetEmployerAccountLegalEntities_Should_Return_Mapped_Legal_Entities(
        string accountHashedId,
        List<Operation> operations,
        GetProviderAccountLegalEntitiesResponse apiResponse,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClientMock,
        AccountLegalEntityPermissionService service)
    {
        // Arrange
        providerRelationshipsApiClientMock
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(
                It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await service.GetEmployerAccountLegalEntities(
            accountHashedId,
            operations);

        // Assert
        result.Should().NotBeNull();
        var item = result.First();
        item.Should().NotBeNull();

        var expectedEntity = apiResponse.AccountProviderLegalEntities.First();

        item.AccountLegalEntityName.Should().Be(expectedEntity.AccountLegalEntityName);
        item.AccountLegalEntityPublicHashedId.Should().Be(expectedEntity.AccountLegalEntityPublicHashedId);
        item.AccountHashedId.Should().Be(expectedEntity.AccountHashedId);
        item.AccountId.Should().Be(expectedEntity.AccountId);
        item.AccountLegalEntityId.Should().Be(expectedEntity.AccountLegalEntityId);
        item.AccountProviderId.Should().Be(expectedEntity.AccountProviderId);
        item.AccountPublicHashedId.Should().Be(expectedEntity.AccountPublicHashedId);
        item.AccountName.Should().Be(expectedEntity.AccountName);
    }

    [Test, MoqAutoData]
    public async Task GetEmployerAccountLegalEntities_Should_Return_Empty_List_When_Response_Is_Null(string accountHashedId,
        List<Operation> operations,
        GetProviderAccountLegalEntitiesResponse apiResponse,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClientMock,
        AccountLegalEntityPermissionService service)
    {
        // Arrange
        providerRelationshipsApiClientMock
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(
                It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync((GetProviderAccountLegalEntitiesResponse)null);

        // Act
        var result = await service.GetEmployerAccountLegalEntities(
            accountHashedId,
            operations);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task GetEmployerAccountLegalEntities_Should_Return_Empty_List_When_AccountProviderLegalEntities_Is_Null(string accountHashedId,
        List<Operation> operations,
        GetProviderAccountLegalEntitiesResponse apiResponse,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClientMock,
        AccountLegalEntityPermissionService service)
    {
        // Arrange
        apiResponse.AccountProviderLegalEntities = null;
        providerRelationshipsApiClientMock
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(
                It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await service.GetEmployerAccountLegalEntities(
            accountHashedId,
            operations);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
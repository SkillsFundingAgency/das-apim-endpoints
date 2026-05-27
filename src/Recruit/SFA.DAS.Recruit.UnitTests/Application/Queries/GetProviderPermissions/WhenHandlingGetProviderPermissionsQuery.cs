using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetProviderPermissions;

[TestFixture]
internal class WhenHandlingGetProviderPermissionsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetProviderPermissionsByUkprnAndAccountIdQuery query,
        List<AccountLegalEntityItem> permissionsResponse,
        GetAccountLegalEntityResponseItem accountLegalEntityResponseItem,
        [Frozen] Mock<IAccountLegalEntityPermissionService> permissionService,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
        GetProviderPermissionsByUkprnQueryHandler handler)
    {
        // Arrange
        var permission = permissionsResponse.First();

        accountLegalEntityResponseItem.AccountLegalEntityId =
            permission.AccountLegalEntityId;

        permissionService
            .Setup(x => x.GetProviderPermissionsForEmployer(
                query.Ukprn,
                query.AccountId,
                It.IsAny<List<Operation>>()))
            .ReturnsAsync(permissionsResponse);

        accountsApi
            .Setup(x => x.GetAll<GetAccountLegalEntityResponseItem>(
                It.IsAny<GetAccountLegalEntitiesRequest>()))
            .ReturnsAsync(new List<GetAccountLegalEntityResponseItem>
            {
                accountLegalEntityResponseItem
            });

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.LegalEntities.Should().HaveCount(1);

        actual.LegalEntities.Should().BeEquivalentTo(
            new List<GetAccountLegalEntityResponseItem>
            {
                accountLegalEntityResponseItem
            });

        accountsApi.Verify(x =>
                x.GetAll<GetAccountLegalEntityResponseItem>(
                    It.IsAny<GetAccountLegalEntitiesRequest>()),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Permissions_Returned_Empty_List_Returned(
        GetProviderPermissionsByUkprnAndAccountIdQuery query,
        [Frozen] Mock<IAccountLegalEntityPermissionService> apiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
        GetProviderPermissionsByUkprnQueryHandler handler)
    {
        // Arrange
        apiClient
            .Setup(x => x.GetProviderPermissionsForEmployer(
                query.Ukprn,
                query.AccountId,
                It.IsAny<List<Operation>>()))
            .ReturnsAsync((List<AccountLegalEntityItem>)null!);

        accountsApi
            .Setup(x => x.GetAll<GetAccountLegalEntityResponseItem>(
                It.IsAny<GetAccountLegalEntitiesRequest>()))
            .ReturnsAsync([]);

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.LegalEntities.Should().NotBeNull();
        actual.LegalEntities.Should().BeEmpty();
    }
}

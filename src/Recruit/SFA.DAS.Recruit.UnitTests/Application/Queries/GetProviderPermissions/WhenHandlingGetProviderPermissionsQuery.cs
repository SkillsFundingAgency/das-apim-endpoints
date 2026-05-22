using SFA.DAS.Recruit.Application.Queries.GetProviderPermissions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetProviderPermissions;

[TestFixture]
internal class WhenHandlingGetProviderPermissionsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetProviderPermissionsByUkprnQuery query,
        List<AccountLegalEntityItem> apiResponse,
        [Frozen] Mock<IAccountLegalEntityPermissionService> apiClient,
        GetProviderPermissionsByUkprnQueryHandler handler)
    {
        //Arrange
        apiClient
            .Setup(x => x.GetProviderPermissionsForEmployer(query.Ukprn, It.IsAny<List<Operation>>())).ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Permissions.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_If_NotFound_Response_Then_Null_Returned(
        GetProviderPermissionsByUkprnQuery query,
        List<AccountLegalEntityItem> apiResponse,
        [Frozen] Mock<IAccountLegalEntityPermissionService> apiClient,
        GetProviderPermissionsByUkprnQueryHandler handler)
    {
        //Arrange
        apiClient
            .Setup(x => x.GetProviderPermissionsForEmployer(query.Ukprn, It.IsAny<List<Operation>>())).ReturnsAsync((List<AccountLegalEntityItem>)null!);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Permissions.Should().BeNull();
    }
}

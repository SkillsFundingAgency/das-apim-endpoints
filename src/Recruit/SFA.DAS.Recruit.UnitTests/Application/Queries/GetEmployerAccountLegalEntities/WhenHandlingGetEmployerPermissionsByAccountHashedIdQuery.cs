using SFA.DAS.Recruit.Application.Queries.GetEmployerAccountLegalEntities;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetEmployerAccountLegalEntities;

[TestFixture]
internal class WhenHandlingGetEmployerPermissionsByAccountHashedIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetEmployerAccountLegalEntitiesQuery query,
        List<LegalEntityItem> permissionsResponse,
        [Frozen] Mock<IAccountLegalEntityPermissionService> permissionService,
        GetEmployerAccountLegalEntitiesQueryHandler handler)
    {
        // Arrange
        permissionService
            .Setup(x => x.GetEmployerAccountLegalEntities(
                query.AccountHashedId,
                It.IsAny<List<Operation>>()))
            .ReturnsAsync(permissionsResponse);

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.LegalEntities.Should().HaveCount(permissionsResponse.Count);

        actual.LegalEntities.Should().BeEquivalentTo(permissionsResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Permissions_Returned_Empty_List_Returned(
        GetEmployerAccountLegalEntitiesQuery query,
        List<LegalEntityItem> permissionsResponse,
        [Frozen] Mock<IAccountLegalEntityPermissionService> permissionService,
        GetEmployerAccountLegalEntitiesQueryHandler handler)
    {
        // Arrange
        permissionService
            .Setup(x => x.GetEmployerAccountLegalEntities(
                query.AccountHashedId,
                It.IsAny<List<Operation>>()))
             .ReturnsAsync((List<LegalEntityItem>)null!);

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.LegalEntities.Should().NotBeNull();
        actual.LegalEntities.Should().BeEmpty();
    }
}
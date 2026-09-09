using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models.Providers.Responses;
using SFA.DAS.Recruit.Application.Queries.GetEmployerAccountLegalEntities;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.AccountLegalEntities;

[TestFixture]
internal class WhenGettingEmployerPermissionsByAccountId
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        string accountHashedId,
        List<Operation> operations,
        GetEmployerAccountLegalEntitiesQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AccountLegalEntitiesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetEmployerAccountLegalEntitiesQuery>(c => c.AccountHashedId.Equals(accountHashedId) && c.Operations.Equals(operations)),
            CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.GetEmployerPermissionsByAccountId(accountHashedId, operations);

        actual.Should().BeOfType<Ok<GetProviderPermissionsApiResponse>>();
        var okResult = actual as Ok<GetProviderPermissionsApiResponse>;
        okResult.Should().NotBeNull();
        okResult.Value!.AccountProviderLegalEntities.Should().BeEquivalentTo(result.LegalEntities);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        string accountHashedId,
        List<Operation> operations,
        GetEmployerAccountLegalEntitiesQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AccountLegalEntitiesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetEmployerAccountLegalEntitiesQuery>(c => c.AccountHashedId.Equals(accountHashedId) && c.Operations.Equals(operations)),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.GetEmployerPermissionsByAccountId(accountHashedId, operations);

        actual.Should().BeOfType<InternalServerError>();
    }
}
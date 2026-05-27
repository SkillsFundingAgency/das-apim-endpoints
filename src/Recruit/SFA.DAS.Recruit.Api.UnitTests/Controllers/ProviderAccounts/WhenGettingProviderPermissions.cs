using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetProviderPermissionsByUkprnAndAccountId;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ProviderAccounts;

[TestFixture]
internal class WhenGettingProviderPermissions
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        int ukprn,
        long accountId,
        GetProviderPermissionsByUkprnAndAccountIdQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderAccountsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetProviderPermissionsByUkprnAndAccountIdQuery>(c => c.Ukprn.Equals(ukprn) && c.AccountId.Equals(accountId)),
            CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.GetProviderPermissions(ukprn, accountId) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual.Value as GetProviderPermissionsByUkprnAndAccountIdQueryResult;
        actualModel.LegalEntities.Should().BeEquivalentTo(result.LegalEntities);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        int ukprn,
        long accountId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderAccountsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetProviderPermissionsByUkprnAndAccountIdQuery>(c => c.Ukprn.Equals(ukprn) && c.AccountId.Equals(accountId)),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.GetProviderPermissions(ukprn, accountId) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
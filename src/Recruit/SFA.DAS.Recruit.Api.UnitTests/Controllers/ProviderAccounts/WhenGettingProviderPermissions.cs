using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetProviderPermissions;
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
        GetProviderPermissionsByUkprnQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderAccountsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetProviderPermissionsByUkprnQuery>(c => c.Ukprn.Equals(ukprn)),
            CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.GetProviderPermissions(ukprn) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual.Value as GetProviderPermissionsResponse;
        actualModel.Permissions.Should().BeEquivalentTo(result.Permissions);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        int ukprn,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProviderAccountsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetProviderPermissionsByUkprnQuery>(c => c.Ukprn.Equals(ukprn)),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.GetProviderPermissions(ukprn) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
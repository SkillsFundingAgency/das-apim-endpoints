using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.ProviderPermissions.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ProviderPermissions;

public class WhenGettingHasRelationshipWithPermission
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        long ukprn,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] ProviderPermissionsController controller,
        CancellationToken cancellationToken)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetHasRelationshipWithPermissionQuery>(c => c.Ukprn.Equals(ukprn)), cancellationToken)).ReturnsAsync(result);

        var actual = await controller.HasRelationshipWithPermission(ukprn, cancellationToken) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual.Value as GetHasRelationshipWithPermissionResponse;
        actualModel.HasPermission.Should().Be(result);
    }
}

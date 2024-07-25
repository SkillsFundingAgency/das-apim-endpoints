using System;
using System.Net;
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
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ProviderPermissions;
public class WhenGettingAccountProviderLegalEntities
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        int ukprn,
        GetProviderAccountLegalEntitiesResponse result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] ProviderPermissionsController controller,
        CancellationToken cancellationToken)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetAccountProviderLegalEntitiesQuery>(c => c.Ukprn.Equals(ukprn)), cancellationToken)).ReturnsAsync(result);

        var actual = await controller.AccountProviderLegalEntities(ukprn, cancellationToken) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual.Value as GetProviderAccountLegalEntitiesResponse;
        actualModel.AccountProviderLegalEntities.Should().BeEquivalentTo(result.AccountProviderLegalEntities);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        int ukprn,
        Operation[] operations,
        GetProviderAccountLegalEntitiesResponse result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] ProviderPermissionsController controller,
        CancellationToken cancellationToken)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetAccountProviderLegalEntitiesQuery>(c => c.Ukprn.Equals(ukprn))
            , cancellationToken)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.AccountProviderLegalEntities(ukprn, cancellationToken) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}

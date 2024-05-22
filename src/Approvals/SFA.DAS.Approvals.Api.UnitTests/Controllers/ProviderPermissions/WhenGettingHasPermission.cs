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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ProviderPermissions;

public class WhenGettingHasPermission
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        long? ukprn, 
        long? accountLegalEntityId,
        Operation operation,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy]ProviderPermissionsController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetHasPermissionQuery>(c => c.Ukprn.Equals(ukprn) && c.AccountLegalEntityId.Equals(accountLegalEntityId) && c.Operation.Equals(operation))
            , CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.HasPermission(ukprn, accountLegalEntityId, operation) as OkObjectResult;
        
        actual.Should().NotBeNull();
        var actualModel = actual.Value as GetHasPermissionResponse;
        actualModel.HasPermission.Should().Be(result);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        long? ukprn, 
        long? accountLegalEntityId,
        Operation operation,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy]ProviderPermissionsController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetHasPermissionQuery>(c => c.Ukprn.Equals(ukprn) && c.AccountLegalEntityId.Equals(accountLegalEntityId) && c.Operation.Equals(operation))
            , CancellationToken.None)).ThrowsAsync(new Exception("Error"));
        
        var actual = await controller.HasPermission(ukprn, accountLegalEntityId, operation) as StatusCodeResult;
        
        actual.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
    }
}
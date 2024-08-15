﻿using System.Threading;
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

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ProviderPermissions
{
    public class WhenGettingHasPermission
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
            bool expected,
            GetHasPermissionQuery query,
            [Frozen] Mock<ISender> mediator,
            [Greedy] ProviderPermissionsController controller,
            CancellationToken cancellationToken)
        {
            mediator.Setup(x => x.Send(query, cancellationToken)).ReturnsAsync(expected);

            var actual = await controller.HasPermission(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

            actual.As<OkObjectResult>().Should().NotBeNull();
            actual.As<OkObjectResult>().Value.As<GetHasPermissionResponse>().HasPermission.Should().Be(expected);
        }
    }
}

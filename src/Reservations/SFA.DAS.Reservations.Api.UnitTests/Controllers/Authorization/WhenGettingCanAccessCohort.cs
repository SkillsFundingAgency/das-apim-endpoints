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
using SFA.DAS.Reservations.Api.Controllers;
using SFA.DAS.Reservations.Application.Cohorts.Queries.GetCohortAccess;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.Api.UnitTests.Controllers.Authorization;

public class WhenGettingCanAccessCohort
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        long providerId,
        long cohortId,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] AuthorizationController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetCohortAccessQuery>(c => c.ProviderId.Equals(providerId) && c.CohortId.Equals(cohortId))
            , CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.CanAccessCohort(providerId, cohortId) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual.Value as GetCohortAccessResponse;
        actualModel.HasCohortAccess.Should().Be(result);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        long providerId,
        long cohortId,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] AuthorizationController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetCohortAccessQuery>(c => c.ProviderId.Equals(providerId) && c.CohortId.Equals(cohortId))
            , CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.CanAccessCohort(providerId, cohortId) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
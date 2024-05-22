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
using SFA.DAS.Approvals.Application.Authorization.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Authorization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Authorization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Authorization;

public class WhenGettingCanAccessCohort
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        Party party,
        long partyId,
        long cohortId,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] AuthorizationController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetCohortAccessQuery>(c => c.Party.Equals(party) && c.PartyId.Equals(partyId) && c.CohortId.Equals(cohortId))
            , CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.CanAccessCohort(partyId, cohortId, party) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual.Value as GetCohortAccessResponse;
        actualModel.HasCohortAccess.Should().Be(result);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        Party party,
        long partyId,
        long cohortId,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] AuthorizationController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetCohortAccessQuery>(c => c.Party.Equals(party) && c.PartyId.Equals(partyId) && c.CohortId.Equals(cohortId))
            , CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.CanAccessCohort(partyId, cohortId, party) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
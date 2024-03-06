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

public class WhenGettingCanAccessApprenticeship
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        Party party,
        long partyId,
        long apprenticeshipId,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] AuthorizationController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetApprenticeshipAccessQuery>(c => c.Party.Equals(party) && c.PartyId.Equals(partyId) && c.ApprenticeshipId.Equals(apprenticeshipId))
            , CancellationToken.None)).ReturnsAsync(result);
        
        var request = new GetApprenticeshipAccessRequest(party, partyId, apprenticeshipId);
        
        var actual = await controller.CanAccessApprenticeship(request) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual.Value as GetApprenticeshipAccessResponse;
        actualModel.HasApprenticeshipAccess.Should().Be(result);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        Party party,
        long partyId,
        long apprenticeshipId,
        bool result,
        [Frozen] Mock<ISender> mediator,
        [Greedy] AuthorizationController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<GetApprenticeshipAccessQuery>(c => c.Party.Equals(party) && c.PartyId.Equals(partyId) && c.ApprenticeshipId.Equals(apprenticeshipId))
            , CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var request = new GetApprenticeshipAccessRequest(party, partyId, apprenticeshipId);
        var actual = await controller.CanAccessApprenticeship(request) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;
public class WhenGettingApplicationSubmitted
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        GetApplicationSubmittedQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetApplicationSubmittedQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId), 
                    CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.ApplicationSubmitted(applicationId, candidateId);

        actual.Should().BeOfType<OkObjectResult>();
        var actualObject = ((OkObjectResult)actual).Value as GetApplicationSubmittedQueryResult;
        actualObject.Should().NotBeNull();
        actualObject.Should().BeEquivalentTo(queryResult);
    }
}

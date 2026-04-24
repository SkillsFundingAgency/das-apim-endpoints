using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;

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

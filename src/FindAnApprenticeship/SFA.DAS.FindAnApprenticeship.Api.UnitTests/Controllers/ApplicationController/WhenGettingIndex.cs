using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController
{
    [TestFixture]
    public class WhenGettingIndex
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            string vacancyReference,
            Guid candidateId,
            Guid applicationId,
            GetIndexQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.ApplicationController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetIndexQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.Index(applicationId, candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetIndexApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo(queryResult, options => options.ExcludingMissingMembers());
        }
    }
}

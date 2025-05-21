using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplicationsCount;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationsController
{
    [TestFixture]
    public class WhenGettingCount
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            ApplicationStatus status,
            GetApplicationsCountQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetApplicationsCountQuery>(q =>
                        q.CandidateId == candidateId && q.Status == status),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.Count(candidateId, status);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetApplicationsCountQueryResult;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo(queryResult);
        }
    }
}

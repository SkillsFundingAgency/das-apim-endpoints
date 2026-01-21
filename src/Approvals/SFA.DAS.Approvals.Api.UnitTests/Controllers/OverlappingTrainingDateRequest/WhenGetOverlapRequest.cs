using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.OverlappingTrainingDateRequest
{
    public class WhenGetOverlapRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Query_To_Get_Overlap_Request(long apprenticeshipId,
            GetOverlapRequestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OverlappingTrainingDateRequestController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetOverlapRequestQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var controllerResult = await controller.GetOverlapRequest(apprenticeshipId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
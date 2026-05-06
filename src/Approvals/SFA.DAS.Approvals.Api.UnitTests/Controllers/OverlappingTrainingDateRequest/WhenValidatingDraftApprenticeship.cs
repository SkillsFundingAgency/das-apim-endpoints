using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.OverlappingTrainingDateRequest
{
    public class WhenValidatingDraftApprenticeship
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Command_To_Validates_DraftApprenticeship(
                  ValidateDraftApprenticeshipRequest request,
                  [Frozen] Mock<IMediator> mockMediator,
                  [Greedy] OverlappingTrainingDateRequestController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<ValidateDraftApprenticeshipDetailsCommand>(x => x.DraftApprenticeshipRequest == request),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.Validate(request) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}

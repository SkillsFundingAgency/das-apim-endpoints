using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.OverlappingTrainingDateRequest
{
    public class WhenValidatingChangeOfEmployerOverlap
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Command_To_Validate_ChangeOfEmployerOverlap(
                  ValidateChangeOfEmployerOverlapRequest request,
                  [Frozen] Mock<IMediator> mockMediator,
                  [Greedy] OverlappingTrainingDateRequestController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<ValidateChangeOfEmployerOverlapCommand>(x => x.ValidateChangeOfEmployerOverlapRequest == request),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.ValidateChangeOfEmployerOverlap(request) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}

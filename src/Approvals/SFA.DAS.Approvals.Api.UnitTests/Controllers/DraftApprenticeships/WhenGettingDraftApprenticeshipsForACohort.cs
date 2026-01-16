using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships
{
    public class WhenGettingDraftApprenticeshipsForACohort
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_DraftApprenticeships_From_Mediator(
             long cohortId,
          GetDraftApprenticeshipsResult mediatorResult,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] DraftApprenticeshipController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetDraftApprenticeshipsQuery>(x => x.CohortId == cohortId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAll(cohortId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetDraftApprenticeshipsResult;
            Assert.That(model, Is.Not.Null);
            model.DraftApprenticeships.Should().BeEquivalentTo(mediatorResult.DraftApprenticeships);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
             long cohortId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DraftApprenticeshipController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetDraftApprenticeshipsQuery>(x => x.CohortId == cohortId),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAll(cohortId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}

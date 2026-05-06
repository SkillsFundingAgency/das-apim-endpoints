using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.Cohorts.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts
{
    public class WhenGettingACohort
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Cohort_From_Mediator(
                    long cohortId,
                    GetCohortResult mediatorResult,
                    [Frozen] Mock<ISender> mockMediator,
                    [Greedy] CohortController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCohortQuery>(x => x.CohortId == cohortId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(cohortId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCohortResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_Cohort_Is_Returned_From_Mediator(
            long cohortId,
            [Frozen] Mock<ISender> mockMediator,
            [Greedy] CohortController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCohortQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var controllerResult = await controller.Get(cohortId) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long cohortId,
            [Frozen] Mock<ISender> mockMediator,
            [Greedy] CohortController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCohortQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(cohortId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}

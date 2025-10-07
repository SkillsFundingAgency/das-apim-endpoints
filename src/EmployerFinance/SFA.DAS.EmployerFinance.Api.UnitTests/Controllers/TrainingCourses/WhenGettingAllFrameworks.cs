using System;
using System.Net;
using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.Application.Queries.GetFrameworks;
namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.TrainingCourses;

public class WhenGettingAllFrameworks
{
        [Test, MoqAutoData]
        public async Task Then_Gets_Frameworks_From_Mediator(
            GetFrameworksQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetFrameworksQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetFrameworks() as ObjectResult;

            controllerResult.Should().NotBeNull();
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetFrameworksResponse;
            model.Should().NotBeNull();
            model.Frameworks.Should().BeEquivalentTo(mediatorResult.Frameworks, options=>options
                .Excluding(c=>c.FundingPeriods)
                .Excluding(c=>c.IsActiveFramework)
                .Excluding(c=>c.CurrentFundingCap)
            );
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetFrameworksQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetFrameworks() as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
}
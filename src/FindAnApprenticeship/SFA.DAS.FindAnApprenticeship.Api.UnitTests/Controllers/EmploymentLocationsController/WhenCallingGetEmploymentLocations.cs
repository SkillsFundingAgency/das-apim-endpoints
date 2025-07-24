using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.EmploymentLocationsController
{
    [TestFixture]
    public class WhenCallingGetEmploymentLocations
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
           Guid candidateId,
           Guid applicationId,
           GetEmploymentLocationsQueryResult queryResult,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.EmploymentLocationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetEmploymentLocationsQuery>(q =>
                        q.CandidateId == candidateId
                        && q.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.Get(applicationId, candidateId);

            using (new AssertionScope())
            {
                actual.Should().BeOfType<OkObjectResult>();
                var actualObject = ((OkObjectResult)actual).Value as GetEmploymentLocationsQueryResult;
                actualObject.Should().NotBeNull();
                actualObject.Should().BeEquivalentTo(queryResult);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Null_Then_Returns_NotFound(
            Guid candidateId,
            Guid applicationId,
            GetEmploymentLocationsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.EmploymentLocationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetEmploymentLocationsQuery>(q =>
                        q.CandidateId == candidateId
                        && q.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetEmploymentLocationsQueryResult)null!);

            var actual = await controller.Get(applicationId, candidateId);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
            Guid candidateId,
            Guid applicationId,
            GetEmploymentLocationsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.EmploymentLocationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetEmploymentLocationsQuery>(q =>
                        q.CandidateId == candidateId
                        && q.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var actual = await controller.Get(applicationId, candidateId);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using SFA.DAS.EmployerIncentives.Application.Commands.AddJobRequest;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.JobControllerTests
{
    [TestFixture]
    public class WhenAddingJobRequest
    {
        [Test, MoqAutoData]
        public async Task Then_AddJobCommand_is_handled(
            long accountId,
            JobRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] JobController controller)
        {

            var controllerResult = await controller.AddJob(request) as StatusCodeResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<AddJobCommand>(c =>
                        c.Type.Equals(request.Type)
                        && c.Data.Equals(request.Data)),
                    It.IsAny<CancellationToken>()));

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Test]
        public async Task Then_a_bad_request_response_is_returned_if_an_http_exception_is_thrown()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var exception = new HttpRequestContentException("Error", HttpStatusCode.BadRequest, "Error message");
            mockMediator.Setup(mediator => mediator.Send(It.IsAny<AddJobCommand>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
            var jobRequest = new Fixture().Build<JobRequest>().Create();
            var controller = new JobController(mockMediator.Object);

            // Act
            var result = await controller.AddJob(jobRequest) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().Be(exception.ErrorContent);
        }
    }
}

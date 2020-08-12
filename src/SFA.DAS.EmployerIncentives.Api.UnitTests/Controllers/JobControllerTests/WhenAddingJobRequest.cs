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
using MediatR;
using SFA.DAS.EmployerIncentives.Application.Commands.AddJobRequest;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

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

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }
}

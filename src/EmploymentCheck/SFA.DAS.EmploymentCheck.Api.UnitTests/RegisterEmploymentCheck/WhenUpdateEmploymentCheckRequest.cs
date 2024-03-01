using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmploymentCheck.Api.Controllers;
using SFA.DAS.EmploymentCheck.Api.Models;
using SFA.DAS.EmploymentCheck.Application.Commands.RegisterCheck;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Api.UnitTests.RegisterEmploymentCheck
{
    [TestFixture]
    public class WhenRegisterEmploymentCheckRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Ok_Result_Returned(
            RegisterCheckRequest request,
            RegisterCheckResponse response,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmploymentCheckController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RegisterCheckCommand>(c =>
                        c.CorrelationId.Equals(request.CorrelationId)
                        && c.ApprenticeshipAccountId.Equals(request.ApprenticeshipAccountId)
                        && c.ApprenticeshipId.Equals(request.ApprenticeshipId)
                        && c.CheckType.Equals(request.CheckType)
                        && c.MaxDate.Equals(request.MaxDate)
                        && c.MinDate.Equals(request.MinDate)
                        && c.Uln.Equals(request.Uln)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var controllerResult = await controller.RegisterCheck(request) as OkObjectResult;
           
            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.Value.Should().Be(response);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Validation_Error_Then_Request_Error_Response_Is_Returned(
            RegisterCheckRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmploymentCheckController controller)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<RegisterCheckCommand>(),
                CancellationToken.None)).ThrowsAsync(new HttpRequestContentException("error message", HttpStatusCode.BadRequest, "error"));

            var actual = await controller.RegisterCheck(request) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            actual.Value.Should().Be("error");
        }
    }
}

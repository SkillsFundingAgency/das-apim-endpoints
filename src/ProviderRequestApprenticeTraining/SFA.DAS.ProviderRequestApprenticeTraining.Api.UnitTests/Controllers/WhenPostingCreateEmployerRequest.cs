using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenPostingCreateEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_EmployerRequestId_Is_Returned_From_Mediator(
            CreateEmployerRequestCommand command,
            CreateEmployerRequestResponse response,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mockMediator
                .Setup(x => x.Send(It.Is<CreateEmployerRequestCommand>(p => p.RequestType == command.RequestType), CancellationToken.None))
                .ReturnsAsync(response);

            // Act
            var actual = await controller.CreateEmployerRequest(command) as ObjectResult;
            
            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(response.EmployerRequestId);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid employerRequestId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<GetEmployerRequestQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetEmployerRequest(employerRequestId) as StatusCodeResult;

            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.AcknowledgeEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenPostingWhenHandlingAcknowledgeEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task Then_Status_Code_Is_Ok_From_Mediator(
            AcknowledgeRequestsParameters param,
            [Greedy] ProvidersController controller)
        {
            // Act
            var actual = await controller.AcknowledgeEmployerRequests(123456,param) as StatusCodeResult;
            
            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            AcknowledgeRequestsParameters param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<AcknowledgeEmployerRequestsCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.AcknowledgeEmployerRequests(123456, param) as StatusCodeResult;

            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

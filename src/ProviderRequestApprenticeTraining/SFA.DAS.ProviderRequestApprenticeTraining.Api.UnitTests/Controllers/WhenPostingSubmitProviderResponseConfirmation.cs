using AutoFixture.NUnit3;
using Azure;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenpostingSubmitproviderresponseConfirmation
    {
        [Test, MoqAutoData]
        public async Task Then_Status_Code_Is_Ok_From_Mediator(
            SubmitProviderResponseCommand command,
            SubmitProviderResponseResponse response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<SubmitProviderResponseCommand>(), CancellationToken.None))
            .ReturnsAsync(response);

            // Act
            var actual = await controller.SubmitProviderResponse(command) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var providerResponse = actual.Value as SubmitProviderResponse;
            providerResponse.ProviderResponseId.Should().Be(response.ProviderResponseId);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            SubmitProviderResponseCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<SubmitProviderResponseCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.SubmitProviderResponse(command) as StatusCodeResult;

            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

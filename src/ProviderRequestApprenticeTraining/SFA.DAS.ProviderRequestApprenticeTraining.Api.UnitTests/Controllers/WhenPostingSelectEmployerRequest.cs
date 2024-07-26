﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenUpdatingProviderResponseStatus
    {
        [Test, MoqAutoData]
        public async Task Then_True_Is_Returned_From_Mediator(
            UpdateProviderResponseStatusCommand command,
            UpdateProviderResponseStatusResponse response,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mockMediator
                .Setup(x => x.Send(It.IsAny<UpdateProviderResponseStatusCommand>(), CancellationToken.None)).ReturnsAsync(response);

            // Act
            var actual = await controller.UpdateProviderResponseStatus(command) as ObjectResult;
            
            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(true);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            UpdateProviderResponseStatusCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<UpdateProviderResponseStatusCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.UpdateProviderResponseStatus(command) as StatusCodeResult;

            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

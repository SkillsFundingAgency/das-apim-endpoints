using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenPostingExpireEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task Then_Status_Code_Is_Ok_From_Mediator(
            [Greedy] EmployerRequestsController controller)
        {
            // Act
            var actual = await controller.ExpireEmployerRequests() as StatusCodeResult;

            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ExpireEmployerRequestsCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.ExpireEmployerRequests() as StatusCodeResult;

            // Assert
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

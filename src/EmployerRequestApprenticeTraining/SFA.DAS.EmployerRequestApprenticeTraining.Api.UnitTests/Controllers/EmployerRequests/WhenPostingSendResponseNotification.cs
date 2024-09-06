using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenPostingSendResponseNotification
    {
        [Test, MoqAutoData]
        public async Task Then_Ok_Is_Returned(
            SendResponseNotificationEmailParameters parameters,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller)
        {
            //Arrange
            mockMediator
                .Setup(m => m.Send(It.IsAny<SendResponseNotificationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            // Act
            var actual = await controller.SendEmployerRequestResponseNotifications(parameters) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            
        }


        [Test, MoqAutoData]
        public async Task And_MediatorCommandIsUnsuccessful_Then_ReturnBadRequest
            (SendResponseNotificationEmailParameters param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<SendResponseNotificationCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.SendEmployerRequestResponseNotifications(param) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        }




    }
}

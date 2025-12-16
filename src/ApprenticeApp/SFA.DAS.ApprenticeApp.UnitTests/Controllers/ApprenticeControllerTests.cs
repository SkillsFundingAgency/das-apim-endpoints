using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Transaction;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.ApprenticeApp.Api.Controllers.ApprenticeController;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class ApprenticeControllerTests
    {
        [Test, MoqAutoData]
        public async Task Get_Apprentice_Test(
            [Greedy] ApprenticeController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            var queryResult = new GetApprenticeQuery
            {
                ApprenticeId = apprenticeId
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprentice(apprenticeId);
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Add_Subscription_Returns_Ok(
            [Greedy] ApprenticeController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var apprenticeAddSubscriptionRequest = new ApprenticeAddSubscriptionRequest() { AuthenticationSecret = "a", Endpoint = "b", PublicKey = "c" };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.ApprenticeAddSubscription(apprenticeId, apprenticeAddSubscriptionRequest) as OkResult;
            result.Should().BeOfType(typeof(OkResult));
        }

        [Test, MoqAutoData]
        public async Task Delete_Subscription_Returns_Ok(
            [Greedy] ApprenticeController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var apprenticeRemoveSubscriptionRequest = new ApprenticeRemoveSubscriptionRequest() { Endpoint = "a" };


            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.ApprenticeRemoveSubscription(apprenticeId, apprenticeRemoveSubscriptionRequest) as OkResult;
            result.Should().BeOfType(typeof(OkResult));
        }

        [Test, MoqAutoData]
        public async Task Delete_Apprentice_Returns_Ok(
     [Frozen] Mock<IMediator> mediatorMock,
     DeleteApprenticeAccountResponse expectedResponse,
     [Greedy] ApprenticeController controller)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var apprenticeId = Guid.NewGuid();

            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<DeleteApprenticeAccountCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await controller.DeleteApprenticeAccountById(apprenticeId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);

            mediatorMock.Verify(m => m.Send(
                It.Is<DeleteApprenticeAccountCommand>(c => c.ApprenticeId == apprenticeId),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
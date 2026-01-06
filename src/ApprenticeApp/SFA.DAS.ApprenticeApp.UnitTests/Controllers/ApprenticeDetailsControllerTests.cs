using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts;
using SFA.DAS.ApprenticeApp.Application.Queries.GetMyApprenticeshipByUln;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Telemetry;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class ApprenticeControllerDetailsTests
    {
        [Test, MoqAutoData]
        public async Task Get_Apprentice_Details_Test(
            [Frozen] Mock<IApprenticeAppMetrics> _apprenticeAppMetrics,
            [Greedy] ApprenticeDetailsController controller)
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

            var result = await controller.GetApprenticeDetails(apprenticeId);
            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task GetApprenticeshipByUln_Returns_Ok_When_Apprenticeship_Exists(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticeDetailsController controller,
            MyApprenticeship myApprenticeship)
        {
            // Arrange
            var uln = 1234567890;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetMyApprenticeshipByUlnQuery>(q => q.Uln == uln),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetMyApprenticeshipByUlnQueryResult
                {
                    MyApprenticeship = myApprenticeship
                });

            // Act
            var result = await controller.GetApprenticeshipByUln(uln);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(myApprenticeship);

            mediatorMock.Verify(m => m.Send(
                It.Is<GetMyApprenticeshipByUlnQuery>(q => q.Uln == uln),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetApprenticeshipByUln_Returns_NotFound_When_Apprenticeship_Does_Not_Exist(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticeDetailsController controller)
        {
            // Arrange
            var uln = 1234567890;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetMyApprenticeshipByUlnQuery>(q => q.Uln == uln),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetMyApprenticeshipByUlnQueryResult
                {
                    MyApprenticeship = null
                });

            // Act
            var result = await controller.GetApprenticeshipByUln(uln);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            mediatorMock.Verify(m => m.Send(
                It.Is<GetMyApprenticeshipByUlnQuery>(q => q.Uln == uln),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
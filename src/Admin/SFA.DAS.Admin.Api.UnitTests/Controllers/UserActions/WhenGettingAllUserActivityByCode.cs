using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Admin.Api.Controllers;
using SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Admin.Api.UnitTests.Controllers.UserActions
{
    public class WhenGettingAllUserActivityByCode
    {
        [Test, MoqAutoData]
        public async Task Then_The_AllUserActivity_Is_Returned(
            string code,
            GetAllUserActivityByCodeQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetAllUserActivityByCodeQuery>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetAllUserActivityByCode(code) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult);

            mediator.Verify(m => m.Send(It.Is<GetAllUserActivityByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returned_If_No_Activity(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetAllUserActivityByCodeQuery>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync((GetAllUserActivityByCodeQueryResult)null);

            // Act
            var actual = await controller.GetAllUserActivityByCode(code) as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            mediator.Verify(m => m.Send(It.Is<GetAllUserActivityByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_Exception(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<GetAllUserActivityByCodeQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetAllUserActivityByCode(code) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetAllUserActivityByCodeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

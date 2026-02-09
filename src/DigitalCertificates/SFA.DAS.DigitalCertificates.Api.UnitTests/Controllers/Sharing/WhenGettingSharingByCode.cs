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
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByCode;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenGettingSharingByCode
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_By_Code_Is_Returned(
            Guid code,
            GetSharingByCodeQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            queryResult.BothFound = false;
            if (queryResult.Response == null)
            {
                queryResult.Response = new Models.SharingByCode
                {
                    CertificateId = Guid.NewGuid(),
                    CertificateType = "Standard",
                    ExpiryTime = DateTime.UtcNow,
                    SharingId = Guid.NewGuid()
                };
            }

            mediator
            .Setup(x => x.Send(It.Is<GetSharingByCodeQuery>(q => q.Code == code), CancellationToken.None))
            .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetSharingByCode(code) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(queryResult.Response);

            mediator.Verify(m => m.Send(It.Is<GetSharingByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Empty_Object_When_No_Response(
            Guid code,
            GetSharingByCodeQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            queryResult.Response = null;
            queryResult.BothFound = false;

            mediator
            .Setup(x => x.Send(It.Is<GetSharingByCodeQuery>(q => q.Code == code), CancellationToken.None))
            .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetSharingByCode(code) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().NotBeNull();
            actual.Value.GetType().GetProperties().Should().BeEmpty();

            mediator.Verify(m => m.Send(It.Is<GetSharingByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_BadRequest_When_BothFound(
            Guid code,
            GetSharingByCodeQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            queryResult.BothFound = true;

            mediator
            .Setup(x => x.Send(It.Is<GetSharingByCodeQuery>(q => q.Code == code), CancellationToken.None))
            .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetSharingByCode(code) as BadRequestResult;

            // Assert
            actual.Should().NotBeNull();

            mediator.Verify(m => m.Send(It.Is<GetSharingByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
            .Setup(x => x.Send(It.IsAny<GetSharingByCodeQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetSharingByCode(code) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetSharingByCodeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

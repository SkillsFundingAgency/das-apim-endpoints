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
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenGettingCertificateSharingDetails
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharings_Are_Returned(
            Guid userId,
            Guid certificateId,
            int limit,
            GetCertificateSharingDetailsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            mediator
                .Setup(x => x.Send(It.Is<GetCertificateSharingDetailsQuery>(q => q.UserId == userId && q.CertificateId == certificateId && q.Limit == limit), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetCertificateSharingDetails(userId, certificateId, limit) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(queryResult.Response);

            mediator.Verify(m => m.Send(It.Is<GetCertificateSharingDetailsQuery>(q => q.UserId == userId && q.CertificateId == certificateId && q.Limit == limit), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            Guid certificateId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetCertificateSharingDetailsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetCertificateSharingDetails(userId, certificateId) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetCertificateSharingDetailsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

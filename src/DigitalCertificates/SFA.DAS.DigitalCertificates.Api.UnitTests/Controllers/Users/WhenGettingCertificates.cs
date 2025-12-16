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
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificates;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingCertificates
    {
        [Test, MoqAutoData]
        public async Task Then_The_Certificates_Are_Returned_From_Mediator(
            Guid userId,
            GetCertificatesResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator
                .Setup(x => x.Send(It.Is<GetCertificatesQuery>(q => q.UserId == userId), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetCertificates(userId) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(queryResult);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetCertificatesQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetCertificates(userId) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

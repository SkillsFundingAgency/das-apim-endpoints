using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.RefreshStandards;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLatestStandards;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.WhenGettingStandard.Api.UnitTests.Controllers.ActiveStandards
{
    public class WhenPuttingRefreshStandards
    {
        [Test, MoqAutoData]
        public async Task Then_The_ActiveStandards_Are_Returned_From_Mediator(
            GetLatestStandardsResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetLatestStandardsQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            mockMediator
                .Setup(x => x.Send(It.IsAny<RefreshStandardsCommand>(), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            var actual = await controller.RefreshStandards() as OkResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
         
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_GetLatestStandardsThrowsException(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetLatestStandardsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.RefreshStandards() as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_RefreshStandardsThrowsException(
            GetLatestStandardsResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetLatestStandardsQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            mockMediator
                .Setup(x => x.Send(It.IsAny<RefreshStandardsCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.RefreshStandards() as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}


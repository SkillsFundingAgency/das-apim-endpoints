using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.Settings
{
    public class WhenGettingSettings
    {
        [Test, MoqAutoData]
        public async Task Then_The_Settings_Are_Returned_From_Mediator(
            GetSettingsResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SettingsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetSettingsQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.Get() as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SettingsController controller)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetSettingsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.Get() as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

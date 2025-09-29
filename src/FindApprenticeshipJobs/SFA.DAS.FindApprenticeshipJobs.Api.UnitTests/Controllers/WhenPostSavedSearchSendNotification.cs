using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenPostSavedSearchSendNotification
    {
        [Test, MoqAutoData]
        public async Task Then_NoContent_Returned_From_Mediator(
            SavedSearchApiRequest mockApiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<PostSendSavedSearchNotificationCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var actual = await sut.SendNotification(mockApiRequest, It.IsAny<CancellationToken>()) as NoContentResult;
            actual.Should().NotBeNull();

            mockMediator.Verify(x => x.Send(It.IsAny<PostSendSavedSearchNotificationCommand>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Exception_Then_InternalServerError_Returned(
            SavedSearchApiRequest mockApiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<PostSendSavedSearchNotificationCommand>(),
                It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var actual = await sut.SendNotification(mockApiRequest, It.IsAny<CancellationToken>()) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

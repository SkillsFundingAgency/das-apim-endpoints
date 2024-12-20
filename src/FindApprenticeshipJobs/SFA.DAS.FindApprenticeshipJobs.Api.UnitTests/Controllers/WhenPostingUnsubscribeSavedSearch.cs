using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.UnsubscribeSavedSearch;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenPostingUnsubscribeSavedSearch
    {
        [Test, MoqAutoData]
        public async Task Then_NoContent_Returned_From_Mediator(
            PostUnsubscribeSavedSearchApiRequest mockApiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchesController sut)
        {
            var actual = await sut.PostUnsubscribeSavedSearch(mockApiRequest, It.IsAny<CancellationToken>()) as ObjectResult;

            mockMediator.Verify(x => x.Send(It.IsAny<UnsubscribeSavedSearchCommand>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
            PostUnsubscribeSavedSearchApiRequest mockApiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<UnsubscribeSavedSearchCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

            var actual = await sut.PostUnsubscribeSavedSearch(mockApiRequest, It.IsAny<CancellationToken>()) as StatusCodeResult;

            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

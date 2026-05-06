using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Api.UnitTests.Extensions;
using SFA.DAS.EmployerFeedback.Application.Queries.GetTrainingProviderSearch;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class EmployerFeedbackControllerTests
    {
        private Mock<IMediator> _mediator;
        private Mock<ILogger<EmployerFeedbackController>> _logger;
        private EmployerFeedbackController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>(MockBehavior.Strict);
            _logger = new Mock<ILogger<EmployerFeedbackController>>();
            _sut = new EmployerFeedbackController(_mediator.Object, _logger.Object);
        }

        [Test]
        public async Task GetTrainingProviderSearch_ReturnsOk_WithMediatorResult()
        {
            // Arrange
            var accountId = 123L;
            var userRef = Guid.NewGuid();
            var expected = new GetTrainingProviderSearchResult { AccountId = accountId, AccountName = "Test" };

            _mediator
                .Setup(m => m.Send(
                    It.Is<GetTrainingProviderSearchQuery>(q => q.AccountId == accountId && q.UserRef == userRef),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var actionResult = await _sut.GetTrainingProviderSearch(accountId, userRef);

            // Assert
            var ok = actionResult as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(ok!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(ok.Value, Is.SameAs(expected));
            });
        }

        [Test]
        public async Task GetTrainingProviderSearch_PassesCorrectParameters_ToMediator()
        {
            // Arrange
            var accountId = 42L;
            var userRef = Guid.NewGuid();
            GetTrainingProviderSearchQuery? captured = null;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetTrainingProviderSearchQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<GetTrainingProviderSearchResult>, CancellationToken>((req, _) => captured = (GetTrainingProviderSearchQuery)req)
                .ReturnsAsync(new GetTrainingProviderSearchResult());

            // Act
            await _sut.GetTrainingProviderSearch(accountId, userRef);

            // Assert
            Assert.That(captured, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(captured!.AccountId, Is.EqualTo(accountId));
                Assert.That(captured.UserRef, Is.EqualTo(userRef));
            });
        }

        [Test]
        public async Task GetTrainingProviderSearch_ReturnsOk_WithNull_WhenMediatorReturnsNull()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetTrainingProviderSearchQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetTrainingProviderSearchResult)null!);

            // Act
            var actionResult = await _sut.GetTrainingProviderSearch(1, Guid.NewGuid());

            // Assert
            var ok = actionResult as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.Value, Is.Null);
        }

        [Test]
        public async Task GetTrainingProviderSearch_WhenMediatorThrows_LogsError_AndReturns500()
        {
            // Arrange
            var boom = new InvalidOperationException("boom");
            _mediator
                .Setup(m => m.Send(It.IsAny<GetTrainingProviderSearchQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(boom);

            // Act
            var result = await _sut.GetTrainingProviderSearch(1, Guid.NewGuid());

            // Assert
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = (StatusCodeResult)result;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

            _logger.VerifyLogErrorContains("Unhandled error get training provider search results.", boom, Times.Once());
        }
    }
}

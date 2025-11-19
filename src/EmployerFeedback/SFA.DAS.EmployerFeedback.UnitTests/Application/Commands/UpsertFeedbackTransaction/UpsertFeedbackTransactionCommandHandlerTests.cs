using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.UpsertFeedbackTransaction;
using SFA.DAS.EmployerFeedback.Configuration;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.UpsertFeedbackTransaction
{
    [TestFixture]
    public class UpsertFeedbackTransactionCommandHandlerTests
    {
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClientMock;
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _employerFeedbackApiClientMock;
        private Mock<ILogger<UpsertFeedbackTransactionCommandHandler>> _loggerMock;
        private Mock<IOptions<EmployerFeedbackConfiguration>> _optionsMock;
        private UpsertFeedbackTransactionCommandHandler _handler;
        private EmployerFeedbackConfiguration _settings;

        [SetUp]
        public void SetUp()
        {
            _commitmentsApiClientMock = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _employerFeedbackApiClientMock = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _loggerMock = new Mock<ILogger<UpsertFeedbackTransactionCommandHandler>>();
            _optionsMock = new Mock<IOptions<EmployerFeedbackConfiguration>>();

            _settings = new EmployerFeedbackConfiguration
            {
                AccountProvidersCourseStatusCompletionLag = 3,
                AccountProvidersCourseStatusStartLag = 60,
                AccountProvidersCourseStatusNewStartWindow = 2
            };

            _optionsMock.Setup(x => x.Value).Returns(_settings);

            _handler = new UpsertFeedbackTransactionCommandHandler(
                _commitmentsApiClientMock.Object,
                _employerFeedbackApiClientMock.Object,
                _loggerMock.Object,
                _optionsMock.Object);
        }

        [Test]
        public async Task Handle_SuccessfullyUpsertsTransaction_WhenValidDataReturned()
        {
            var accountId = 12345L;
            var command = new UpsertFeedbackTransactionCommand { AccountId = accountId };

            var accountProvidersCourseStatusResponse = new GetAccountProvidersCourseStatusResponse
            {
                Active = new List<AccountProviderCourse>
                {
                    new AccountProviderCourse { Ukprn = 10023829, CourseCode = "430" }
                },
                Completed = new List<AccountProviderCourse>
                {
                    new AccountProviderCourse { Ukprn = 10007431, CourseCode = "167" }
                },
                NewStart = new List<AccountProviderCourse>
                {
                    new AccountProviderCourse { Ukprn = 10012467, CourseCode = "532" }
                }
            };

            var commitmentsApiResponse = new ApiResponse<GetAccountProvidersCourseStatusResponse>(
                accountProvidersCourseStatusResponse, HttpStatusCode.OK, string.Empty);

            var feedbackApiResponse = new ApiResponse<object>(
                null, HttpStatusCode.NoContent, string.Empty);

            _commitmentsApiClientMock
                .Setup(x => x.GetWithResponseCode<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(commitmentsApiResponse);

            _employerFeedbackApiClientMock
                .Setup(x => x.PostWithResponseCode<UpsertFeedbackTransactionData, object>(It.IsAny<UpsertFeedbackTransactionRequest>(), false))
                .ReturnsAsync(feedbackApiResponse);

            await _handler.Handle(command, CancellationToken.None);

            _commitmentsApiClientMock.Verify(x => x.GetWithResponseCode<GetAccountProvidersCourseStatusResponse>(
                It.Is<GetAccountProvidersCourseStatusRequest>(r =>
                    r.AccountId == accountId &&
                    r.CompletionLag == _settings.AccountProvidersCourseStatusCompletionLag &&
                    r.StartLag == _settings.AccountProvidersCourseStatusStartLag &&
                    r.NewStartWindow == _settings.AccountProvidersCourseStatusNewStartWindow)), Times.Once);

            _employerFeedbackApiClientMock.Verify(x => x.PostWithResponseCode<UpsertFeedbackTransactionData, object>(
                It.Is<UpsertFeedbackTransactionRequest>(r => r.AccountId == accountId), false), Times.Once);
        }

        [Test]
        public async Task Handle_PassesEmptyDataToFeedbackApi_WhenResponseHasEmptyLists()
        {
            var accountId = 12345L;
            var command = new UpsertFeedbackTransactionCommand { AccountId = accountId };

            var accountProvidersCourseStatusResponse = new GetAccountProvidersCourseStatusResponse
            {
                Active = new List<AccountProviderCourse>(),
                Completed = new List<AccountProviderCourse>(),
                NewStart = new List<AccountProviderCourse>()
            };

            var commitmentsApiResponse = new ApiResponse<GetAccountProvidersCourseStatusResponse>(
                accountProvidersCourseStatusResponse, HttpStatusCode.OK, string.Empty);

            var feedbackApiResponse = new ApiResponse<object>(
                null, HttpStatusCode.NoContent, string.Empty);

            _commitmentsApiClientMock
                .Setup(x => x.GetWithResponseCode<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(commitmentsApiResponse);

            _employerFeedbackApiClientMock
                .Setup(x => x.PostWithResponseCode<UpsertFeedbackTransactionData, object>(It.IsAny<UpsertFeedbackTransactionRequest>(), false))
                .ReturnsAsync(feedbackApiResponse);

            await _handler.Handle(command, CancellationToken.None);

            _commitmentsApiClientMock.Verify(x => x.GetWithResponseCode<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()), Times.Once);
            _employerFeedbackApiClientMock.Verify(x => x.PostWithResponseCode<UpsertFeedbackTransactionData, object>(
                It.Is<UpsertFeedbackTransactionRequest>(r =>
                    r.AccountId == accountId &&
                    r.Data.Active.Count == 0 &&
                    r.Data.Completed.Count == 0 &&
                    r.Data.Completed.Count == 0 &&
                    r.Data.Completed.Count == 0 &&
                    r.Data.NewStart.Count == 0), false), Times.Once);
        }

        [Test]
        public void Handle_ThrowsException_WhenCommitmentsApiCallFails()
        {
            var accountId = 12345L;
            var command = new UpsertFeedbackTransactionCommand { AccountId = accountId };

            var commitmentsApiResponse = new ApiResponse<GetAccountProvidersCourseStatusResponse>(
                null, HttpStatusCode.InternalServerError, "Error");

            _commitmentsApiClientMock
                .Setup(x => x.GetWithResponseCode<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(commitmentsApiResponse);

            Assert.ThrowsAsync<SharedOuterApi.Exceptions.ApiResponseException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            _commitmentsApiClientMock.Verify(x => x.GetWithResponseCode<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()), Times.Once);
            _employerFeedbackApiClientMock.Verify(x => x.PostWithResponseCode<UpsertFeedbackTransactionData, object>(It.IsAny<UpsertFeedbackTransactionRequest>(), false), Times.Never);
        }
    }
}
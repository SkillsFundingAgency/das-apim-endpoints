using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.SyncEmployerAccounts
{
    [TestFixture]
    public class SyncEmployerAccountsCommandHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _feedbackApiClientMock;
        private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClientMock;
        private Mock<ILogger<SyncEmployerAccountsCommandHandler>> _loggerMock;
        private SyncEmployerAccountsCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _feedbackApiClientMock = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _accountsApiClientMock = new Mock<IAccountsApiClient<AccountsConfiguration>>();
            _loggerMock = new Mock<ILogger<SyncEmployerAccountsCommandHandler>>();
            _handler = new SyncEmployerAccountsCommandHandler(
                _feedbackApiClientMock.Object,
                _accountsApiClientMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task Handle_WhenNoAccountsReturned_DoesNotCallUpsertAccountsData()
        {
            _accountsApiClientMock.Setup(x => x.GetWithResponseCode<GetUpdatedEmployerAccountsResponse>(It.IsAny<GetUpdatedEmployerAccountsRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetUpdatedEmployerAccountsResponse>(
                    null, HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.GetWithResponseCode<GetRefreshALELastRunDateSettingResponse>(It.IsAny<GetRefreshALELastRunDateSettingRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetRefreshALELastRunDateSettingResponse>(new GetRefreshALELastRunDateSettingResponse(), HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.Put<UpsertRefreshALELastRunDateSettingData>(It.IsAny<UpsertRefreshALELastRunDateSettingRequest>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(new SyncEmployerAccountsCommand(), CancellationToken.None);

            _feedbackApiClientMock.Verify(x => x.PostWithResponseCode<AccountsData, object>(It.IsAny<UpsertAccountsRequest>(), false), Times.Never);
        }

        [Test]
        public async Task Handle_WhenAccountsReturned_CallsUpsertAndUpdateSettings()
        {
            var updatedAccounts = new List<UpdatedEmployerAccounts> { new UpdatedEmployerAccounts { AccountId = 1, AccountName = "Test" } };
            _accountsApiClientMock.Setup(x => x.GetWithResponseCode<GetUpdatedEmployerAccountsResponse>(It.IsAny<GetUpdatedEmployerAccountsRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetUpdatedEmployerAccountsResponse>(
                    new GetUpdatedEmployerAccountsResponse { Data = updatedAccounts, Page = 1, TotalPages = 1 }, HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.GetWithResponseCode<GetRefreshALELastRunDateSettingResponse>(It.IsAny<GetRefreshALELastRunDateSettingRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetRefreshALELastRunDateSettingResponse>(new GetRefreshALELastRunDateSettingResponse(), HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.PostWithResponseCode<AccountsData, object>(It.IsAny<UpsertAccountsRequest>(), false))
                .ReturnsAsync(new SFA.DAS.SharedOuterApi.Models.ApiResponse<object>(null, HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.Put<UpsertRefreshALELastRunDateSettingData>(It.IsAny<UpsertRefreshALELastRunDateSettingRequest>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(new SyncEmployerAccountsCommand(), CancellationToken.None);

            _feedbackApiClientMock.Verify(x => x.PostWithResponseCode<AccountsData, object>(It.IsAny<UpsertAccountsRequest>(), false), Times.Once);
            _feedbackApiClientMock.Verify(x => x.Put<UpsertRefreshALELastRunDateSettingData>(It.IsAny<UpsertRefreshALELastRunDateSettingRequest>()), Times.Once);
        }

        [Test]
        public async Task Handle_WhenAccountsResponseBodyIsNull_DoesNotCallUpsertOrUpdateSyncDate()
        {
            _accountsApiClientMock.Setup(x => x.GetWithResponseCode<GetUpdatedEmployerAccountsResponse>(It.IsAny<GetUpdatedEmployerAccountsRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetUpdatedEmployerAccountsResponse>(
                    null, HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.GetWithResponseCode<GetRefreshALELastRunDateSettingResponse>(It.IsAny<GetRefreshALELastRunDateSettingRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetRefreshALELastRunDateSettingResponse>(new GetRefreshALELastRunDateSettingResponse(), HttpStatusCode.OK, null));

            await _handler.Handle(new SyncEmployerAccountsCommand(), CancellationToken.None);

            _feedbackApiClientMock.Verify(x => x.PostWithResponseCode<AccountsData, object>(It.IsAny<UpsertAccountsRequest>(), false), Times.Never);
            _feedbackApiClientMock.Verify(x => x.Put<UpsertRefreshALELastRunDateSettingData>(It.IsAny<UpsertRefreshALELastRunDateSettingRequest>()), Times.Never);
        }

        [Test]
        public async Task Handle_WhenAccountsResponseDataIsNull_DoesNotCallUpsertOrUpdateSyncDate()
        {
            _accountsApiClientMock.Setup(x => x.GetWithResponseCode<GetUpdatedEmployerAccountsResponse>(It.IsAny<GetUpdatedEmployerAccountsRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetUpdatedEmployerAccountsResponse>(
                    new GetUpdatedEmployerAccountsResponse { Data = null, Page = 1, TotalPages = 1 }, HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.GetWithResponseCode<GetRefreshALELastRunDateSettingResponse>(It.IsAny<GetRefreshALELastRunDateSettingRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetRefreshALELastRunDateSettingResponse>(new GetRefreshALELastRunDateSettingResponse(), HttpStatusCode.OK, null));

            await _handler.Handle(new SyncEmployerAccountsCommand(), CancellationToken.None);

            _feedbackApiClientMock.Verify(x => x.PostWithResponseCode<AccountsData, object>(It.IsAny<UpsertAccountsRequest>(), false), Times.Never);
            _feedbackApiClientMock.Verify(x => x.Put<UpsertRefreshALELastRunDateSettingData>(It.IsAny<UpsertRefreshALELastRunDateSettingRequest>()), Times.Never);
        }

        [Test]
        public async Task Handle_WhenAccountsResponseDataIsEmpty_UpdatesSyncDateButDoesNotCallUpsert()
        {
            _accountsApiClientMock.Setup(x => x.GetWithResponseCode<GetUpdatedEmployerAccountsResponse>(It.IsAny<GetUpdatedEmployerAccountsRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetUpdatedEmployerAccountsResponse>(
                    new GetUpdatedEmployerAccountsResponse { Data = new List<UpdatedEmployerAccounts>(), Page = 1, TotalPages = 1 }, HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.GetWithResponseCode<GetRefreshALELastRunDateSettingResponse>(It.IsAny<GetRefreshALELastRunDateSettingRequest>()))
                .ReturnsAsync(new SharedOuterApi.Models.ApiResponse<GetRefreshALELastRunDateSettingResponse>(new GetRefreshALELastRunDateSettingResponse(), HttpStatusCode.OK, null));
            _feedbackApiClientMock.Setup(x => x.Put<UpsertRefreshALELastRunDateSettingData>(It.IsAny<UpsertRefreshALELastRunDateSettingRequest>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(new SyncEmployerAccountsCommand(), CancellationToken.None);

            _feedbackApiClientMock.Verify(x => x.PostWithResponseCode<AccountsData, object>(It.IsAny<UpsertAccountsRequest>(), false), Times.Never);
            _feedbackApiClientMock.Verify(x => x.Put<UpsertRefreshALELastRunDateSettingData>(It.IsAny<UpsertRefreshALELastRunDateSettingRequest>()), Times.Once);
        }
    }
}

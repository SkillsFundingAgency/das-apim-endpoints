using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionUsers;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.EmployerFeedback.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetFeedbackTransactionUsers
{
    public class GetFeedbackTransactionUsersQueryHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _employerFeedbackApiClientMock;
        private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClientMock;
        private Mock<ILogger<GetFeedbackTransactionUsersQueryHandler>> _loggerMock;
        private GetFeedbackTransactionUsersQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _employerFeedbackApiClientMock = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _accountsApiClientMock = new Mock<IAccountsApiClient<AccountsConfiguration>>();
            _loggerMock = new Mock<ILogger<GetFeedbackTransactionUsersQueryHandler>>();

            _handler = new GetFeedbackTransactionUsersQueryHandler(
                _employerFeedbackApiClientMock.Object,
                _accountsApiClientMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsUsers_WhenValidFeedbackTransactionAndEligibleUsers()
        {
            const long feedbackTransactionId = 123L;
            const long accountId = 456L;
            var sendAfter = DateTime.UtcNow.AddDays(-1);

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = accountId,
                AccountName = "Test Account",
                TemplateName = "Test Template",
                SendAfter = sendAfter,
                SentDate = null
            };

            var accountUsers = new List<GetAccountTeamMembersResponse>
            {
                new GetAccountTeamMembersResponse
                {
                    FirstName = "John",
                    Email = "john@example.com",
                    CanReceiveNotifications = true,
                    Status = 2
                },
                new GetAccountTeamMembersResponse
                {
                    FirstName = "Jane",
                    Email = "jane@example.com",
                    CanReceiveNotifications = true,
                    Status = 2
                }
            };

            var feedbackTransactionResponse = new ApiResponse<GetFeedbackTransactionResponse>(
                feedbackTransaction, HttpStatusCode.OK, string.Empty);

            _employerFeedbackApiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(
                    It.Is<GetFeedbackTransactionRequest>(r => r.FeedbackTransactionId == feedbackTransactionId)))
                .ReturnsAsync(feedbackTransactionResponse);

            _accountsApiClientMock
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(r => r.GetAllUrl == $"api/accounts/internal/{accountId}/users")))
                .ReturnsAsync(accountUsers);

            var query = new GetFeedbackTransactionUsersQuery(feedbackTransactionId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccountId, Is.EqualTo(accountId));
            Assert.That(result.AccountName, Is.EqualTo("Test Account"));
            Assert.That(result.TemplateName, Is.EqualTo("Test Template"));
            Assert.That(result.Users, Is.Not.Null);

            var usersList = result.Users as List<FeedbackTransactionUser> ?? new List<FeedbackTransactionUser>(result.Users);
            Assert.That(usersList.Count, Is.EqualTo(2));
            Assert.That(usersList[0].Name, Is.EqualTo("John"));
            Assert.That(usersList[0].Email, Is.EqualTo("john@example.com"));
            Assert.That(usersList[1].Name, Is.EqualTo("Jane"));
            Assert.That(usersList[1].Email, Is.EqualTo("jane@example.com"));
        }

        [Test]
        public void Handle_ThrowsException_WhenFeedbackTransactionNotFound()
        {
            const long feedbackTransactionId = 123L;

            var feedbackTransactionResponse = new ApiResponse<GetFeedbackTransactionResponse>(
                null, HttpStatusCode.OK, string.Empty);

            _employerFeedbackApiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(
                    It.Is<GetFeedbackTransactionRequest>(r => r.FeedbackTransactionId == feedbackTransactionId)))
                .ReturnsAsync(feedbackTransactionResponse);

            var query = new GetFeedbackTransactionUsersQuery(feedbackTransactionId);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _handler.Handle(query, CancellationToken.None));

            Assert.That(ex.Message, Is.EqualTo($"Feedback transaction {feedbackTransactionId} not found"));
        }

        [Test]
        public void Handle_ThrowsException_WhenSendAfterDateIsInFuture()
        {
            const long feedbackTransactionId = 123L;
            const long accountId = 456L;
            var sendAfter = DateTime.UtcNow.AddDays(1);

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = accountId,
                AccountName = "Test Account",
                TemplateName = "Test Template",
                SendAfter = sendAfter,
                SentDate = null
            };

            var feedbackTransactionResponse = new ApiResponse<GetFeedbackTransactionResponse>(
                feedbackTransaction, HttpStatusCode.OK, string.Empty);

            _employerFeedbackApiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(
                    It.Is<GetFeedbackTransactionRequest>(r => r.FeedbackTransactionId == feedbackTransactionId)))
                .ReturnsAsync(feedbackTransactionResponse);

            var query = new GetFeedbackTransactionUsersQuery(feedbackTransactionId);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _handler.Handle(query, CancellationToken.None));

            Assert.That(ex.Message, Is.EqualTo("Feedback transaction sendAfter date is in the future"));
        }

        [Test]
        public async Task Handle_ReturnsNull_WhenFeedbackTransactionAlreadySent()
        {
            const long feedbackTransactionId = 123L;
            const long accountId = 456L;
            var sendAfter = DateTime.UtcNow.AddDays(-1);
            var sentDate = DateTime.UtcNow.AddHours(-1);

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = accountId,
                AccountName = "Test Account",
                TemplateName = "Test Template",
                SendAfter = sendAfter,
                SentDate = sentDate
            };

            var feedbackTransactionResponse = new ApiResponse<GetFeedbackTransactionResponse>(
                feedbackTransaction, HttpStatusCode.OK, string.Empty);

            _employerFeedbackApiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(
                    It.Is<GetFeedbackTransactionRequest>(r => r.FeedbackTransactionId == feedbackTransactionId)))
                .ReturnsAsync(feedbackTransactionResponse);

            var query = new GetFeedbackTransactionUsersQuery(feedbackTransactionId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Handle_FiltersOutIneligibleUsers_WhenUsersHaveVariousStatuses()
        {
            const long feedbackTransactionId = 123L;
            const long accountId = 456L;
            var sendAfter = DateTime.UtcNow.AddDays(-1);

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = accountId,
                AccountName = "Test Account",
                TemplateName = "Test Template",
                SendAfter = sendAfter,
                SentDate = null
            };

            var accountUsers = new List<GetAccountTeamMembersResponse>
            {
                new GetAccountTeamMembersResponse
                {
                    FirstName = "John",
                    Email = "john@example.com",
                    CanReceiveNotifications = true,
                    Status = 2
                },
                new GetAccountTeamMembersResponse
                {
                    FirstName = "Jane",
                    Email = "jane@example.com",
                    CanReceiveNotifications = false,
                    Status = 2
                },
                new GetAccountTeamMembersResponse
                {
                    FirstName = "Bob",
                    Email = "bob@example.com",
                    CanReceiveNotifications = true,
                    Status = 1
                },
                new GetAccountTeamMembersResponse
                {
                    FirstName = "Alice",
                    Email = "alice@example.com",
                    CanReceiveNotifications = true,
                    Status = 2
                }
            };

            var feedbackTransactionResponse = new ApiResponse<GetFeedbackTransactionResponse>(
                feedbackTransaction, HttpStatusCode.OK, string.Empty);

            _employerFeedbackApiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(
                    It.Is<GetFeedbackTransactionRequest>(r => r.FeedbackTransactionId == feedbackTransactionId)))
                .ReturnsAsync(feedbackTransactionResponse);

            _accountsApiClientMock
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(r => r.GetAllUrl == $"api/accounts/internal/{accountId}/users")))
                .ReturnsAsync(accountUsers);

            var query = new GetFeedbackTransactionUsersQuery(feedbackTransactionId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Users, Is.Not.Null);

            var usersList = result.Users as List<FeedbackTransactionUser> ?? new List<FeedbackTransactionUser>(result.Users);
            Assert.That(usersList.Count, Is.EqualTo(2));
            Assert.That(usersList[0].Name, Is.EqualTo("John"));
            Assert.That(usersList[1].Name, Is.EqualTo("Alice"));
        }

        [Test]
        public async Task Handle_ReturnsEmptyUsersList_WhenNoEligibleUsers()
        {
            const long feedbackTransactionId = 123L;
            const long accountId = 456L;
            var sendAfter = DateTime.UtcNow.AddDays(-1);

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = accountId,
                AccountName = "Test Account",
                TemplateName = "Test Template",
                SendAfter = sendAfter,
                SentDate = null
            };

            var accountUsers = new List<GetAccountTeamMembersResponse>();

            var feedbackTransactionResponse = new ApiResponse<GetFeedbackTransactionResponse>(
                feedbackTransaction, HttpStatusCode.OK, string.Empty);

            _employerFeedbackApiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(
                    It.Is<GetFeedbackTransactionRequest>(r => r.FeedbackTransactionId == feedbackTransactionId)))
                .ReturnsAsync(feedbackTransactionResponse);

            _accountsApiClientMock
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(r => r.GetAllUrl == $"api/accounts/internal/{accountId}/users")))
                .ReturnsAsync(accountUsers);

            var query = new GetFeedbackTransactionUsersQuery(feedbackTransactionId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccountId, Is.EqualTo(accountId));
            Assert.That(result.Users, Is.Not.Null);

            var usersList = result.Users as List<FeedbackTransactionUser> ?? new List<FeedbackTransactionUser>(result.Users);
            Assert.That(usersList.Count, Is.EqualTo(0));
        }
    }
}
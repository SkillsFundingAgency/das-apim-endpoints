using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.CanUserReceiveNotifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingCanUserReceiveNotificationsQuery
    {
        private Mock<IAccountsApiClient<AccountsConfiguration>> _mockAccountsApiClient;
        private CanUserReceiveNotificationsQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockAccountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
            _handler = new CanUserReceiveNotificationsQueryHandler(_mockAccountsApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_TeamMemberExistsAndCanReceiveNotoficationsIsTrue_True_Is_Returned(Guid userRef, long accountId)
        {
            // Arrange
            CanUserReceiveNotificationsQuery query = new CanUserReceiveNotificationsQuery 
            {
                AccountId = accountId,
                UserId = userRef,
            };
            
            var apiTeamResponse = new List<GetAccountTeamMembersResponse> 
            { 
                new GetAccountTeamMembersResponse
                {
                    CanReceiveNotifications = true,
                    UserRef = userRef.ToString(),
                }
            };

            _mockAccountsApiClient
                .Setup(client => client.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
            .ReturnsAsync(apiTeamResponse);

            // Act
            var actual = await _handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_TeamMemberExistsAndCanReceiveNotoficationsIsFalse_False_Is_Returned(Guid userRef, long accountId)
        {
            // Arrange
            CanUserReceiveNotificationsQuery query = new CanUserReceiveNotificationsQuery
            {
                AccountId = accountId,
                UserId = userRef,
            };

            var apiTeamResponse = new List<GetAccountTeamMembersResponse>
            {
                new GetAccountTeamMembersResponse
                {
                    CanReceiveNotifications = false,
                    UserRef = userRef.ToString(),
                }
            };

            _mockAccountsApiClient
                .Setup(client => client.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
            .ReturnsAsync(apiTeamResponse);

            // Act
            var actual = await _handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_TeamMemberNotFound(Guid userRef, long accountId)
        {
            // Arrange
            CanUserReceiveNotificationsQuery query = new CanUserReceiveNotificationsQuery
            {
                AccountId = accountId,
                UserId = userRef,
            };

            var apiTeamResponse = new List<GetAccountTeamMembersResponse>();

            _mockAccountsApiClient
                .Setup(client => client.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
            .ReturnsAsync(apiTeamResponse);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }


        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CanUserReceiveNotificationsQuery query)
        {
            // Arrange
            _mockAccountsApiClient
                .Setup(client => client.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}

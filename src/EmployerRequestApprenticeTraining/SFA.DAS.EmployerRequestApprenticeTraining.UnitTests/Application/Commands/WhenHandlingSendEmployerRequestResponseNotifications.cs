using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.Encoding;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingSendEmployerRequestResponseNotifications
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockTrainingApiClient;
        private Mock<IAccountsApiClient<AccountsConfiguration>> _mockAccountsApiClient;
        private Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> _mockEmployerProfilesApiClient;
        private Mock<IEncodingService> _mockEncodingService;
        private Mock<INotificationService> _mockNotificationsService;
        private Mock<IOptions<EmployerRequestApprenticeTrainingConfiguration>> _mockOptions;
        private SendResponseNotificationCommandHandler _handler;
        private readonly string _encodedAccountId = "ABCDE";
        

        [SetUp]
        public void Arrange()
        {
            _mockTrainingApiClient = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();
            _mockAccountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
            _mockEmployerProfilesApiClient = new Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>>();
            _mockNotificationsService = new Mock<INotificationService>();
            
            _mockEncodingService = new Mock<IEncodingService>();
            _mockEncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.AccountId)).Returns(_encodedAccountId);

            _mockOptions = new Mock<IOptions<EmployerRequestApprenticeTrainingConfiguration>>();

            var config = new EmployerRequestApprenticeTrainingConfiguration
            {
                NotificationTemplates = new List<NotificationTemplate>
                {
                    new NotificationTemplate
                    {
                        TemplateName = "RATProviderResponseConfirmation",
                        TemplateId = Guid.NewGuid()
                    }
                }
            };
            _mockOptions.Setup(o => o.Value).Returns(config);

            _handler = new SendResponseNotificationCommandHandler(
                _mockAccountsApiClient.Object, 
                _mockEmployerProfilesApiClient.Object, 
                _mockNotificationsService.Object, 
                _mockEncodingService.Object,
                _mockOptions.Object);        
        }

        [Test, MoqAutoData]
        public async Task Then_IfRequestorCanReceiveNotificationsIsTrue_EmailIsSentToRequestor()
        {
            // Arrange
            var requestedByUserId = Guid.NewGuid();

            var employerProfilesResponse = new EmployerProfileUsersApiResponse 
            { 
                Id = requestedByUserId.ToString(),
                FirstName = "Kim",
                LastName = "King",
                DisplayName = "Kim King",
                Email= "kimking@king.com",
            };

            var profileApiResponse = new ApiResponse<EmployerProfileUsersApiResponse>(employerProfilesResponse, HttpStatusCode.OK, string.Empty);
            _mockEmployerProfilesApiClient.Setup(c => c.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()))
                .ReturnsAsync(profileApiResponse);

            var teamMembersResponse = new List<GetAccountTeamMembersResponse> 
            { 
                new GetAccountTeamMembersResponse{ Email = employerProfilesResponse.Email, CanReceiveNotifications = true, UserRef = employerProfilesResponse.Id },
                new GetAccountTeamMembersResponse{ Email = "anyemail@google.com", CanReceiveNotifications = false, UserRef = Guid.NewGuid().ToString()},
                new GetAccountTeamMembersResponse{ Email = "anotherl@google.com", CanReceiveNotifications = true, UserRef = Guid.NewGuid().ToString()}
            };
            _mockAccountsApiClient.Setup(c => c.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
                .ReturnsAsync(teamMembersResponse);

            SendResponseNotificationCommand command = new SendResponseNotificationCommand 
            { 
                AccountId = 123456,
                RequestedBy = requestedByUserId,
                Standards = new List<StandardDetails> 
                { 
                    new StandardDetails { StandardTitle = "Title 1", StandardLevel = 1},
                    new StandardDetails { StandardTitle = "Title 2", StandardLevel = 2},
                }
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            var templateId = _mockOptions.Object.Value.NotificationTemplates.First().TemplateId.ToString();

            _mockEmployerProfilesApiClient.Verify(c => c.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()), Times.Once);
            _mockAccountsApiClient.Verify(c => c.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()), Times.Once);
            _mockNotificationsService.Verify(n => n.Send(It.Is<SendEmailCommand>(c => c.TemplateId == templateId && c.RecipientsAddress == employerProfilesResponse.Email)), Times.Once); 
        }

        [Test, MoqAutoData]
        public async Task Then_IfRequestorCanReceiveNotificationsIsFalse_EmailIsNotSentToRequestor()
        {
            // Arrange
            var requestedByUserId = Guid.NewGuid();

            var employerProfilesResponse = new EmployerProfileUsersApiResponse
            {
                Id = requestedByUserId.ToString(),
                FirstName = "Kim",
                LastName = "King",
                DisplayName = "Kim King",
                Email = "kimking@king.com",
            };

            var profileApiResponse = new ApiResponse<EmployerProfileUsersApiResponse>(employerProfilesResponse, HttpStatusCode.OK, string.Empty);
            _mockEmployerProfilesApiClient.Setup(c => c.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()))
                .ReturnsAsync(profileApiResponse);

            var teamMembersResponse = new List<GetAccountTeamMembersResponse>
            {
                new GetAccountTeamMembersResponse{ Email = employerProfilesResponse.Email, CanReceiveNotifications = false, UserRef = employerProfilesResponse.Id },
                new GetAccountTeamMembersResponse{ Email = "anyemail@google.com", CanReceiveNotifications = false, UserRef = Guid.NewGuid().ToString()},
                new GetAccountTeamMembersResponse{ Email = "anotherl@google.com", CanReceiveNotifications = true, UserRef = Guid.NewGuid().ToString()}
            };
            _mockAccountsApiClient.Setup(c => c.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
                .ReturnsAsync(teamMembersResponse);

            SendResponseNotificationCommand command = new SendResponseNotificationCommand
            {
                AccountId = 123456,
                RequestedBy = requestedByUserId,
                Standards = new List<StandardDetails>
                {
                    new StandardDetails { StandardTitle = "Title 1", StandardLevel = 1},
                    new StandardDetails { StandardTitle = "Title 2", StandardLevel = 2},
                }
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            var templateId = _mockOptions.Object.Value.NotificationTemplates.First().TemplateId.ToString();

            _mockEmployerProfilesApiClient.Verify(c => c.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()), Times.Once);
            _mockAccountsApiClient.Verify(c => c.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()), Times.Once);
            _mockNotificationsService.Verify(n => n.Send(It.Is<SendEmailCommand>(c => c.TemplateId == templateId && c.RecipientsAddress == employerProfilesResponse.Email)), Times.Never);
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_EmployerProfileApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            SendResponseNotificationCommand command,
            string errorContent)
        {
            // Arrange
            var requestedByUserId = Guid.NewGuid();

            var employerProfilesResponse = new EmployerProfileUsersApiResponse
            {
                Id = requestedByUserId.ToString(),
                FirstName = "Kim",
                LastName = "King",
                DisplayName = "Kim King",
                Email = "kimking@king.com",
            };

            var profileApiResponse = new ApiResponse<EmployerProfileUsersApiResponse>(employerProfilesResponse, statusCode, errorContent);
            _mockEmployerProfilesApiClient.Setup(c => c.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()))
                .ReturnsAsync(profileApiResponse);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }

        [Test, MoqAutoData]
        public async Task And_AccountsApiReturnsEmptyTeamList_Then_ThrowApiResponseException(
            SendResponseNotificationCommand command,
            string errorContent)
        {
            // Arrange
            var requestedByUserId = Guid.NewGuid();

            var employerProfilesResponse = new EmployerProfileUsersApiResponse
            {
                Id = requestedByUserId.ToString(),
                FirstName = "Kim",
                LastName = "King",
                DisplayName = "Kim King",
                Email = "kimking@king.com",
            };

            var profileApiResponse = new ApiResponse<EmployerProfileUsersApiResponse>(employerProfilesResponse, HttpStatusCode.OK, errorContent);
            _mockEmployerProfilesApiClient.Setup(c => c.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()))
                .ReturnsAsync(profileApiResponse);

            _mockAccountsApiClient.Setup(c => c.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>());

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }

        [Test, MoqAutoData]
        public async Task And_AccountsApiReturnsNull_Then_ThrowApiResponseException(
            SendResponseNotificationCommand command,
            string errorContent)
        {
            // Arrange
            var requestedByUserId = Guid.NewGuid();

            var employerProfilesResponse = new EmployerProfileUsersApiResponse
            {
                Id = requestedByUserId.ToString(),
                FirstName = "Kim",
                LastName = "King",
                DisplayName = "Kim King",
                Email = "kimking@king.com",
            };

            var profileApiResponse = new ApiResponse<EmployerProfileUsersApiResponse>(employerProfilesResponse, HttpStatusCode.OK, errorContent);
            _mockEmployerProfilesApiClient.Setup(c => c.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()))
                .ReturnsAsync(profileApiResponse);

            _mockAccountsApiClient.Setup(c => c.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
                .ReturnsAsync((List<GetAccountTeamMembersResponse>)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }

        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Not_Sent_If_Template_Not_Found()
        {
            // Arrange
            var requestedByUserId = Guid.NewGuid();

            var employerProfilesResponse = new EmployerProfileUsersApiResponse
            {
                Id = requestedByUserId.ToString(),
                FirstName = "Kim",
                LastName = "King",
                DisplayName = "Kim King",
                Email = "kimking@king.com",
            };

            var profileApiResponse = new ApiResponse<EmployerProfileUsersApiResponse>(employerProfilesResponse, HttpStatusCode.OK, string.Empty);
            _mockEmployerProfilesApiClient.Setup(c => c.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()))
                .ReturnsAsync(profileApiResponse);

            var teamMembersResponse = new List<GetAccountTeamMembersResponse>
            {
                new GetAccountTeamMembersResponse{ Email = employerProfilesResponse.Email, CanReceiveNotifications = true, UserRef = employerProfilesResponse.Id },
                new GetAccountTeamMembersResponse{ Email = "anyemail@google.com", CanReceiveNotifications = false, UserRef = Guid.NewGuid().ToString()},
                new GetAccountTeamMembersResponse{ Email = "anotherl@google.com", CanReceiveNotifications = true, UserRef = Guid.NewGuid().ToString()}
            };
            _mockAccountsApiClient.Setup(c => c.GetAll<GetAccountTeamMembersResponse>(It.IsAny<GetAccountTeamMembersRequest>()))
                .ReturnsAsync(teamMembersResponse);

            var emptyNotificationTemplates = new EmployerRequestApprenticeTrainingConfiguration
            {
                NotificationTemplates = new List<NotificationTemplate>()
            };
            _mockOptions.Setup(o => o.Value).Returns(emptyNotificationTemplates);

            SendResponseNotificationCommand command = new SendResponseNotificationCommand
            {
                AccountId = 123456,
                RequestedBy = requestedByUserId,
                Standards = new List<StandardDetails>
                {
                    new StandardDetails { StandardTitle = "Title 1", StandardLevel = 1},
                    new StandardDetails { StandardTitle = "Title 2", StandardLevel = 2},
                }
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockNotificationsService.Verify(n => n.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        }
    }
}

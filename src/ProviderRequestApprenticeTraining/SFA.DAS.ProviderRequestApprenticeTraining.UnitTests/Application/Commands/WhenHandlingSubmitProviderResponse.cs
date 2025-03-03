using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.ProviderRequestApprenticeTraining.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingSubmitproviderResponse
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockApiClient;
        private Mock<INotificationService> _mockNotificationsService;
        private Mock<IOptions<ProviderRequestApprenticeTrainingConfiguration>> _mockOptions;
        private SubmitProviderResponseCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();
            _mockNotificationsService = new Mock<INotificationService>();
            _mockOptions = new Mock<IOptions<ProviderRequestApprenticeTrainingConfiguration>>();

            var notificationTemplates = new ProviderRequestApprenticeTrainingConfiguration
            {
                NotificationTemplates = new List<NotificationTemplate>
                {
                    new NotificationTemplate
                    {
                        TemplateName = EmailTemplateNames.RATProviderResponseConfirmation,
                        TemplateId = Guid.NewGuid()
                    }
                }
            };
            _mockOptions.Setup(o => o.Value).Returns(notificationTemplates);

            _handler = new SubmitProviderResponseCommandHandler(_mockApiClient.Object, _mockNotificationsService.Object, _mockOptions.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(SubmitProviderResponseCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<SubmitProviderResponseResponse>(new SubmitProviderResponseResponse(), HttpStatusCode.Created, errorContent);
        
            _mockApiClient.Setup(c => c.PostWithResponseCode<SubmitProviderResponseRequestData, SubmitProviderResponseResponse>(It.IsAny<SubmitProviderResponseRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockApiClient.Verify(c => c.PostWithResponseCode<SubmitProviderResponseRequestData, SubmitProviderResponseResponse>(It.IsAny<SubmitProviderResponseRequest>(), It.IsAny<bool>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestSentIsCommand(SubmitProviderResponseCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<SubmitProviderResponseResponse>(new SubmitProviderResponseResponse(), HttpStatusCode.Created, errorContent);
            IPostApiRequest<SubmitProviderResponseRequestData> submittedRequest = null;

            _mockApiClient.Setup(c => c.PostWithResponseCode<SubmitProviderResponseRequestData, SubmitProviderResponseResponse>(It.IsAny<SubmitProviderResponseRequest>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest<SubmitProviderResponseRequestData>, bool>((x, y) => submittedRequest = x)
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            submittedRequest.Data.Should().BeEquivalentTo(new
            {
                command.Ukprn,
            });
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            SubmitProviderResponseCommand command,
            string errorContent)
        {
            var response = new ApiResponse<SubmitProviderResponseResponse>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<SubmitProviderResponseRequestData, SubmitProviderResponseResponse>(It.IsAny<SubmitProviderResponseRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }

        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Sent_If_Template_Found(SubmitProviderResponseCommand command, 
            SubmitProviderResponseResponse response, 
            string errorContent)
        {
            // Arrange
            var apiResponse = new ApiResponse<SubmitProviderResponseResponse>(response, HttpStatusCode.Created, errorContent);
            _mockApiClient.Setup(c => c.PostWithResponseCode<SubmitProviderResponseRequestData, SubmitProviderResponseResponse>(It.IsAny<SubmitProviderResponseRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(apiResponse);

            var templateId = _mockOptions.Object.Value.NotificationTemplates.First().TemplateId.ToString();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            var expectedCourseLevel = string.Format("{0} (level {1})", command.StandardTitle, command.StandardLevel);

            _mockNotificationsService.Verify(n => n.Send(It.Is<SendEmailCommand>(c => 
                c.TemplateId == templateId && 
                c.RecipientsAddress == command.CurrentUserEmail &&
                c.Tokens.GetValueOrDefault("user_name") == command.CurrentUserFirstName &&
                c.Tokens.GetValueOrDefault("course_level") == expectedCourseLevel
                )), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Not_Sent_If_Template_Not_Found(SubmitProviderResponseCommand command, SubmitProviderResponseResponse response, string errorContent)
        {
            // Arrange
            var apiResponse = new ApiResponse<SubmitProviderResponseResponse>(response, HttpStatusCode.Created, errorContent);
            _mockApiClient.Setup(c => c.PostWithResponseCode<SubmitProviderResponseRequestData, SubmitProviderResponseResponse>(It.IsAny<SubmitProviderResponseRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(apiResponse);

            var emptyNotificationTemplates = new ProviderRequestApprenticeTrainingConfiguration
            {
                NotificationTemplates = new List<NotificationTemplate>()
            };
            _mockOptions.Setup(o => o.Value).Returns(emptyNotificationTemplates);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockNotificationsService.Verify(n => n.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        }
    }
}

using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.Notifications.Messages.Commands;
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
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.PostSubmitEmployerRequestRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingSubmitEmployerRequest
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockApiClient;
        private Mock<INotificationService> _mockNotificationsService;
        private Mock<IOptions<EmployerRequestApprenticeTrainingConfiguration>> _mockOptions;
        private SubmitEmployerRequestCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();
            _mockNotificationsService = new Mock<INotificationService>();
            _mockOptions = new Mock<IOptions<EmployerRequestApprenticeTrainingConfiguration>>();

            var notificationTemplates = new EmployerRequestApprenticeTrainingConfiguration
            {
                NotificationTemplates = new List<NotificationTemplate>
                {
                    new NotificationTemplate
                    {
                        TemplateName = "SubmitEmployerRequest",
                        TemplateId = Guid.NewGuid()
                    }
                }
            };
            _mockOptions.Setup(o => o.Value).Returns(notificationTemplates);

            _handler = new SubmitEmployerRequestCommandHandler(_mockApiClient.Object, _mockNotificationsService.Object, _mockOptions.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(SubmitEmployerRequestCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<SubmitEmployerRequestResponse>(new SubmitEmployerRequestResponse(), HttpStatusCode.Created, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<PostSubmitEmployerRequestData, SubmitEmployerRequestResponse>(It.IsAny<PostSubmitEmployerRequestRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockApiClient.Verify(c => c.PostWithResponseCode<PostSubmitEmployerRequestData, SubmitEmployerRequestResponse>(It.IsAny<PostSubmitEmployerRequestRequest>(), It.IsAny<bool>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestSentIsCommand(SubmitEmployerRequestCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<SubmitEmployerRequestResponse>(new SubmitEmployerRequestResponse(), HttpStatusCode.Created, errorContent);
            IPostApiRequest<PostSubmitEmployerRequestData> submittedRequest = null;

            _mockApiClient.Setup(c => c.PostWithResponseCode<PostSubmitEmployerRequestData, SubmitEmployerRequestResponse>(It.IsAny<PostSubmitEmployerRequestRequest>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest<PostSubmitEmployerRequestData>, bool>((x, y) => submittedRequest = x)
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            submittedRequest.Data.Should().BeEquivalentTo(new
            {
                command.RequestType
            });
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            SubmitEmployerRequestCommand command,
            string errorContent)
        {
            var response = new ApiResponse<SubmitEmployerRequestResponse>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<PostSubmitEmployerRequestData, SubmitEmployerRequestResponse>(It.IsAny<PostSubmitEmployerRequestRequest>(), true))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }

        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Sent_If_Template_Found(SubmitEmployerRequestCommand command, SubmitEmployerRequestResponse response, string errorContent)
        {
            // Arrange
            var apiResponse = new ApiResponse<SubmitEmployerRequestResponse>(response, HttpStatusCode.Created, errorContent);
            _mockApiClient.Setup(c => c.PostWithResponseCode<PostSubmitEmployerRequestData, SubmitEmployerRequestResponse>(It.IsAny<PostSubmitEmployerRequestRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(apiResponse);

            var templateId = _mockOptions.Object.Value.NotificationTemplates.First().TemplateId.ToString();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockNotificationsService.Verify(n => n.Send(It.Is<SendEmailCommand>(c => c.TemplateId == templateId && c.RecipientsAddress == command.RequestedByEmail)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Not_Sent_If_Template_Not_Found(SubmitEmployerRequestCommand command, SubmitEmployerRequestResponse response, string errorContent)
        {
            // Arrange
            var apiResponse = new ApiResponse<SubmitEmployerRequestResponse>(response, HttpStatusCode.Created, errorContent);
            _mockApiClient.Setup(c => c.PostWithResponseCode<PostSubmitEmployerRequestData, SubmitEmployerRequestResponse>(It.IsAny<PostSubmitEmployerRequestRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(apiResponse);

            var emptyNotificationTemplates = new EmployerRequestApprenticeTrainingConfiguration
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

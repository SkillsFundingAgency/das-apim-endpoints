using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingCancelEmployerRequest
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockApiClient;
        private Mock<INotificationService> _mockNotificationsService;
        private Mock<IOptions<EmployerRequestApprenticeTrainingConfiguration>> _mockOptions;
        private CancelEmployerRequestCommandHandler _handler;

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
                        TemplateName = EmailTemplateNames.RATEmployerCancelConfirmation,
                        TemplateId = Guid.NewGuid()
                    }
                }
            };
            _mockOptions.Setup(o => o.Value).Returns(notificationTemplates);

            _handler = new CancelEmployerRequestCommandHandler(_mockApiClient.Object, _mockNotificationsService.Object, _mockOptions.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PutRequestIsSent(CancelEmployerRequestCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, errorContent);

            _mockApiClient.Setup(c => c.PutWithResponseCode<PutCancelEmployerRequestRequestData, NullResponse>(It.IsAny<PutCancelEmployerRequestRequest>()))
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockApiClient.Verify(c => c.PutWithResponseCode<PutCancelEmployerRequestRequestData, NullResponse>(It.IsAny<PutCancelEmployerRequestRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_PutRequestSentIsCommand(CancelEmployerRequestCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, errorContent);
            IPutApiRequest<PutCancelEmployerRequestRequestData> cancelledRequest = null;

            _mockApiClient.Setup(c => c.PutWithResponseCode<PutCancelEmployerRequestRequestData, NullResponse>(It.IsAny<PutCancelEmployerRequestRequest>()))
                .Callback<IPutApiRequest<PutCancelEmployerRequestRequestData>>((x) => cancelledRequest = x)
                .ReturnsAsync(response);
                
            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            cancelledRequest.Data.Should().BeEquivalentTo(new
            {
                command.CancelledBy
            });
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            CancelEmployerRequestCommand command,
            string errorContent)
        {
            var response = new ApiResponse<NullResponse>(new NullResponse(), statusCode, errorContent);
            _mockApiClient.Setup(c => c.PutWithResponseCode<PutCancelEmployerRequestRequestData, NullResponse>(It.IsAny<PutCancelEmployerRequestRequest>()))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }

        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Sent_If_Template_Found(CancelEmployerRequestCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, errorContent);
            _mockApiClient.Setup(c => c.PutWithResponseCode<PutCancelEmployerRequestRequestData, NullResponse>(It.IsAny<PutCancelEmployerRequestRequest>()))
                .ReturnsAsync(response);

            var templateId = _mockOptions.Object.Value.NotificationTemplates
                .First(p => p.TemplateName == EmailTemplateNames.RATEmployerCancelConfirmation).TemplateId.ToString();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockNotificationsService.Verify(n => n.Send(It.Is<SendEmailCommand>(c => c.TemplateId == templateId && c.RecipientsAddress == command.CancelledByEmail)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Not_Sent_If_Template_Not_Found(CancelEmployerRequestCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, errorContent);
            _mockApiClient.Setup(c => c.PutWithResponseCode<PutCancelEmployerRequestRequestData, NullResponse>(It.IsAny<PutCancelEmployerRequestRequest>()))
                .ReturnsAsync(response);

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


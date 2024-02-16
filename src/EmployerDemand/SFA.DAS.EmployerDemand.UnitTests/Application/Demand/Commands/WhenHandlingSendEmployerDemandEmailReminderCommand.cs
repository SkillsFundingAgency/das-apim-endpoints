using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.SendEmployerDemandReminder;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Commands
{
    public class WhenHandlingSendEmployerDemandEmailReminderCommand
    {
        [Test, MoqAutoData]
        public async Task Then_A_Notification_Is_Sent_And_Api_Called(
            GetEmployerDemandResponse response,
            SendEmployerDemandEmailReminderCommand command,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> employerDemandApiClient,
            [Frozen] Mock<INotificationService> notificationService,
            SendEmployerDemandEmailReminderCommandHandler handler)
        {
            //Arrange
            employerDemandApiClient.Setup(x =>
                x.Get<GetEmployerDemandResponse>(It.Is<GetEmployerDemandRequest>(c =>
                    c.GetUrl.Contains(command.EmployerDemandId.ToString()))))
                .ReturnsAsync(response);
            SendEmailCommand actualEmail = null;
            notificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Callback((SendEmailCommand args) => actualEmail = args)
                .Returns(Task.CompletedTask);
            employerDemandApiClient.Setup(
                    x => x.PostWithResponseCode<object>(It.IsAny<PostEmployerDemandNotificationAuditRequest>(), true))
                .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.Accepted, null));
            var expectedEmail = new CreateEmployerDemandReminderEmail(
                response.ContactEmailAddress,
                response.OrganisationName,
                response.Course.Title, 
                response.Course.Level,
                response.Location.Name,
                response.NumberOfApprentices,
                response.StopSharingUrl);
            
            
            //Act
            await handler.Handle(command, CancellationToken.None);
            
            //Assert
            employerDemandApiClient.Verify(
                x => x.PostWithResponseCode<object>(It.Is<PostEmployerDemandNotificationAuditRequest>(c =>
                    c.PostUrl.Contains($"{command.EmployerDemandId}/notification-audit/{command.Id}?notificationType={(short)NotificationType.Reminder}")),true), Times.Once);
            actualEmail.Tokens.Should().BeEquivalentTo(expectedEmail.Tokens);
            actualEmail.RecipientsAddress.Should().BeEquivalentTo(expectedEmail.RecipientAddress);
            actualEmail.TemplateId.Should().BeEquivalentTo(expectedEmail.TemplateId);
        }
    }
}
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.SendAutomaticEmployerDemandDemandCutOff;
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
    public class WhenHandlingSendAutomaticEmployerDemandDemandCutOffCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_EmailSent_And_Demand_Updated(
            GetEmployerDemandResponse response,
            SendAutomaticEmployerDemandDemandCutOffCommand command,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> employerDemandApiClient,
            [Frozen] Mock<INotificationService> notificationService,
            SendAutomaticEmployerDemandDemandCutOffCommandHandler handler)
        {
            //Arrange
            employerDemandApiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(It.Is<GetEmployerDemandRequest>(c =>
                        c.GetUrl.Contains(command.EmployerDemandId.ToString()))))
                .ReturnsAsync(response);
            employerDemandApiClient.Setup(
                x => x.PostWithResponseCode<object>(It.Is<PostEmployerDemandNotificationAuditRequest>(c =>
                    c.PostUrl.Contains($"{command.EmployerDemandId}/notification-audit/{command.Id}?notificationType={(short)NotificationType.StoppedAutomaticCutOff}")),true)).ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.Accepted,""));
            employerDemandApiClient.Setup(
                x => x.PatchWithResponseCode(It.Is<PatchCourseDemandRequest>(c =>
                    c.PatchUrl.Contains($"api/demand/{command.EmployerDemandId}") 
                    && c.Data.FirstOrDefault().Path.Equals("Stopped")
                    && c.Data.FirstOrDefault().Value.Equals(true)
                ))).ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.Accepted,""));
            SendEmailCommand actualEmail = null;
            notificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Callback((SendEmailCommand args) => actualEmail = args)
                .Returns(Task.CompletedTask);
            var expectedEmail = new StopSharingExpiredEmployerDemandEmail(
                response.ContactEmailAddress,
                response.OrganisationName,
                response.Course.Title, 
                response.Course.Level,
                response.Location.Name,
                response.NumberOfApprentices, 
                response.StartSharingUrl);
            
            //Act
            await handler.Handle(command, CancellationToken.None);
            
            //Assert
            employerDemandApiClient.Verify(
                x => x.PostWithResponseCode<object>(It.Is<PostEmployerDemandNotificationAuditRequest>(c =>
                    c.PostUrl.Contains($"{command.EmployerDemandId}/notification-audit/{command.Id}?notificationType={(short)NotificationType.StoppedAutomaticCutOff}")),true), Times.Once);
            employerDemandApiClient.Verify(
                x => x.PatchWithResponseCode(It.Is<PatchCourseDemandRequest>(c =>
                    c.PatchUrl.Contains($"api/demand/{command.EmployerDemandId}") 
                    && c.Data.FirstOrDefault().Path.Equals("Stopped")
                    && c.Data.FirstOrDefault().Value.Equals(true)
                    )), Times.Once);
            actualEmail.Tokens.Should().BeEquivalentTo(expectedEmail.Tokens);
            actualEmail.RecipientsAddress.Should().BeEquivalentTo(expectedEmail.RecipientAddress);
            actualEmail.TemplateId.Should().BeEquivalentTo(expectedEmail.TemplateId);
        }
    }
}
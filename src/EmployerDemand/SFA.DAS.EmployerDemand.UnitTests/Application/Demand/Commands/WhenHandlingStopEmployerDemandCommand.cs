using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Commands
{
    public class WhenHandlingStopEmployerDemandCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Email_Sent_If_ResponseCode_Is_Ok_And_Audit_Created(
            StopEmployerDemandCommand command,
            GetEmployerDemandResponse getDemandResponse,
            GetEmployerDemandResponse stopResponseBody,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockApiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            StopEmployerDemandCommandHandler handler)
        {
            //Arrange
            getDemandResponse.Stopped = false;
            var stopResponse = new ApiResponse<string>(JsonConvert.SerializeObject(stopResponseBody), HttpStatusCode.OK, "");
            mockApiClient
                .Setup(client => client.PatchWithResponseCode(
                    It.Is<PatchCourseDemandRequest>(c=>c.PatchUrl.Contains(command.EmployerDemandId.ToString()) 
                                                       && c.Data.FirstOrDefault().Path.Equals("Stopped")
                                                       && c.Data.FirstOrDefault().Value.Equals(true)
                                                       )))
                .ReturnsAsync(stopResponse);
            mockApiClient
                .Setup(client => client.Get<GetEmployerDemandResponse>(
                    It.Is<GetEmployerDemandRequest>(request => request.GetUrl.Contains($"demand/{command.EmployerDemandId}"))))
                .ReturnsAsync(getDemandResponse);

            SendEmailCommand actualEmail = null;
            mockNotificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Callback((SendEmailCommand args) => actualEmail = args)
                .Returns(Task.CompletedTask);
            var expectedEmail = new StopSharingEmployerDemandEmail(
                stopResponseBody.ContactEmailAddress,
                stopResponseBody.OrganisationName,
                stopResponseBody.Course.Title,
                stopResponseBody.Course.Level,
                stopResponseBody.Location.Name,
                stopResponseBody.NumberOfApprentices, 
                stopResponseBody.StartSharingUrl);

            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(stopResponseBody);
            actualEmail.Tokens.Should().BeEquivalentTo(expectedEmail.Tokens);
            actualEmail.RecipientsAddress.Should().BeEquivalentTo(expectedEmail.RecipientAddress);
            actualEmail.TemplateId.Should().BeEquivalentTo(expectedEmail.TemplateId);
            mockApiClient.Verify(
                x => x.PostWithResponseCode<object>(It.Is<PostEmployerDemandNotificationAuditRequest>(c =>
                    c.PostUrl.Contains($"{command.EmployerDemandId}/notification-audit/{command.Id}?notificationType={(short)NotificationType.StoppedByUser}")),true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Demand_Not_Found_Then_Not_Send_Email(
            StopEmployerDemandCommand command,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockApiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            StopEmployerDemandCommandHandler handler)
        {
            //Arrange
            mockApiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{command.EmployerDemandId}"))))
                .ReturnsAsync((GetEmployerDemandResponse)null);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            mockApiClient.Verify(client => client.PatchWithResponseCode(It.IsAny<PatchCourseDemandRequest>()), 
                Times.Never);
            mockNotificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), 
                Times.Never);
            actual.EmployerDemand.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task And_Demand_Already_Stopped_Then_Not_Send_Email(
            StopEmployerDemandCommand command,
            GetEmployerDemandResponse getDemandResponse,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockApiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            StopEmployerDemandCommandHandler handler)
        {
            //Arrange
            getDemandResponse.Stopped = true;
            mockApiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{command.EmployerDemandId}"))))
                .ReturnsAsync(getDemandResponse);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            mockApiClient.Verify(client => client.PatchWithResponseCode(It.IsAny<PatchCourseDemandRequest>()), 
                Times.Never);
            mockNotificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), 
                Times.Never);
            actual.EmployerDemand.Should().BeEquivalentTo(getDemandResponse);
        }

        [Test, MoqAutoData]
        public void Then_If_Error_For_Verify_An_Exception_Is_Thrown(
            string errorContent,
            StopEmployerDemandCommand command,
            GetEmployerDemandResponse getDemandResponse,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockApiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            StopEmployerDemandCommandHandler handler)
        {
            //Arrange
            getDemandResponse.Stopped = false;
            var stopResponse = new ApiResponse<string>(null, HttpStatusCode.InternalServerError, errorContent);
            mockApiClient
                .Setup(client => client.PatchWithResponseCode(
                    It.Is<PatchCourseDemandRequest>(request => request.PatchUrl.Contains(command.EmployerDemandId.ToString()))))
                .ReturnsAsync(stopResponse);
            mockApiClient
                .Setup(client => client.Get<GetEmployerDemandResponse>(
                    It.Is<GetEmployerDemandRequest>(request => request.GetUrl.Contains($"demand/{command.EmployerDemandId}"))))
                .ReturnsAsync(getDemandResponse);

            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            //Assert
            act.Should().ThrowAsync<HttpRequestContentException>().WithMessage($"Response status code does not indicate success: {(int)HttpStatusCode.InternalServerError} ({HttpStatusCode.InternalServerError})").Result
                .Which.ErrorContent.Should().Be(errorContent);
            mockNotificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), 
                Times.Never);
        }
    }
}
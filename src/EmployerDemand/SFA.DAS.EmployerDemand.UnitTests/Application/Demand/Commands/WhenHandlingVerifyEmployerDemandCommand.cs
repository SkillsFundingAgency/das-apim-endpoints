using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand;
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
    public class WhenHandlingVerifyEmployerDemandCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Api_Called(
            VerifyEmployerDemandCommand command,
            string patchResponse,
            GetEmployerDemandResponse getDemandResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen] Mock<INotificationService> notificationService,
            VerifyEmployerDemandCommandHandler handler)
        {
            //Arrange
            getDemandResponse.EmailVerified = false;
            var apiResponse = new ApiResponse<string>(patchResponse, HttpStatusCode.OK, null);
            
            apiClient.Setup(
                x => x.PatchWithResponseCode(It.Is<PatchCourseDemandRequest>(c =>
                    c.PatchUrl.Contains($"api/demand/{command.Id}") 
                    && c.Data.FirstOrDefault().Path.Equals("EmailVerified")
                    && c.Data.FirstOrDefault().Value.Equals(true)
                ))).ReturnsAsync(apiResponse);
            
            apiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{command.Id}"))))
                .ReturnsAsync(getDemandResponse);
            SendEmailCommand actualEmail = null;
            notificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Callback((SendEmailCommand args) => actualEmail = args)
                .Returns(Task.CompletedTask);
            var expectedEmail = new CreateDemandConfirmationEmail(
                getDemandResponse.ContactEmailAddress,
                getDemandResponse.OrganisationName,
                getDemandResponse.Course.Title, 
                getDemandResponse.Course.Level,
                getDemandResponse.Location.Name,
                getDemandResponse.NumberOfApprentices,
                getDemandResponse.StopSharingUrl);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(getDemandResponse, options=>options.Excluding(c=>c.EmailVerified));
            actual.EmployerDemand.EmailVerified.Should().BeTrue();
            actualEmail.Tokens.Should().BeEquivalentTo(expectedEmail.Tokens);
            actualEmail.RecipientsAddress.Should().BeEquivalentTo(expectedEmail.RecipientAddress);
            actualEmail.TemplateId.Should().BeEquivalentTo(expectedEmail.TemplateId);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Email_Is_Not_Sent_If_Already_Verified_On_Get_Demand(
            VerifyEmployerDemandCommand command,
            PostEmployerCourseDemand verifyEmailResponse,
            GetEmployerDemandResponse getDemandResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen] Mock<INotificationService> notificationService,
            VerifyEmployerDemandCommandHandler handler)
        {
            //Arrange
            getDemandResponse.EmailVerified = true;
            apiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{command.Id}"))))
                .ReturnsAsync(getDemandResponse);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            apiClient.Verify(client => client.PatchWithResponseCode(
                    It.IsAny<PatchCourseDemandRequest>()), Times.Never);
            notificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), 
                Times.Never);
            actual.EmployerDemand.Should().BeEquivalentTo(getDemandResponse);
        }
        
        [Test, MoqAutoData]
        public async Task And_Demand_Not_Found_At_Api_Then_Return_Null_EmployerDemand(
            VerifyEmployerDemandCommand command,
            PostEmployerCourseDemand verifyEmailResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen] Mock<INotificationService> notificationService,
            VerifyEmployerDemandCommandHandler handler)
        {
            //Arrange
            apiClient
                .Setup(x => x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => 
                            c.GetUrl.Contains($"demand/{command.Id}"))))
                .ReturnsAsync((GetEmployerDemandResponse)null);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            apiClient.Verify(client => client.PatchWithResponseCode(
                It.IsAny<PatchCourseDemandRequest>()), Times.Never);
            notificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), 
                Times.Never);
            actual.EmployerDemand.Should().BeNull();
        }
        
        [Test, MoqAutoData]
        public async Task And_Demand_Anonymised_Then_EmployerDemand_Returned(
            VerifyEmployerDemandCommand command,
            PostEmployerCourseDemand verifyEmailResponse,
            GetEmployerDemandResponse getDemandResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen] Mock<INotificationService> notificationService,
            VerifyEmployerDemandCommandHandler handler)
        {
            //Arrange
            getDemandResponse.ContactEmailAddress = string.Empty;
            apiClient
                .Setup(x => x.Get<GetEmployerDemandResponse>(
                    It.Is<GetEmployerDemandRequest>(c => 
                        c.GetUrl.Contains($"demand/{command.Id}"))))
                .ReturnsAsync(getDemandResponse);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            apiClient.Verify(client => client.PatchWithResponseCode(
                It.IsAny<PatchCourseDemandRequest>()), Times.Never);
            notificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), 
                Times.Never);
            actual.EmployerDemand.Should().BeEquivalentTo(getDemandResponse);
        }

        [Test, MoqAutoData]
        public void Then_If_Error_For_Verify_An_Exception_Is_Thrown(
            string errorContent,
            GetEmployerDemandResponse getDemandResponse,
            VerifyEmployerDemandCommand command,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen] Mock<INotificationService> notificationService,
            VerifyEmployerDemandCommandHandler handler)
        {
            //Arrange
            getDemandResponse.EmailVerified = false;
            apiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{command.Id}"))))
                .ReturnsAsync(getDemandResponse);
            var apiResponse = new ApiResponse<string>("patchResponse", HttpStatusCode.BadRequest, errorContent);
            
            apiClient.Setup(
                x => x.PatchWithResponseCode(It.IsAny<PatchCourseDemandRequest>())).ReturnsAsync(apiResponse);

            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            //Assert
            act.Should().ThrowAsync<HttpRequestContentException>().WithMessage($"Response status code does not indicate success: {(int)HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})").Result
                .Which.ErrorContent.Should().Be(errorContent);
            notificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), 
                Times.Never);
        }
    }
}
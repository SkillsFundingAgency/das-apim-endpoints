using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.ProviderInterest.Commands
{
    public class WhenHandlingCreateProviderInterestsCommand
    {
        [Test, MoqAutoData]
        public async Task And_ResponseCode_Is_Created_Then_Sends_Emails(
            CreateProviderInterestsCommand command,
            PostCreateProviderInterestsResponse responseBody,
            List<GetEmployerDemandResponse> employerDemandsFromApi,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockApiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            CreateProviderInterestsCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<PostCreateProviderInterestsResponse>(responseBody, HttpStatusCode.Created, null);
            mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateProviderInterestsResponse>(It.IsAny<PostCreateProviderInterestsRequest>()))
                .ReturnsAsync(apiResponse);
            mockNotificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Returns(Task.CompletedTask);
            mockApiClient
                .SetupSequence(client => client.Get<GetEmployerDemandResponse>(It.IsAny<GetEmployerDemandRequest>()))
                .ReturnsAsync(employerDemandsFromApi[0])
                .ReturnsAsync(employerDemandsFromApi[1])
                .ReturnsAsync(employerDemandsFromApi[2]);

            //Act
            var response = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            response.Should().Be(responseBody.Id);

            var employerDemandIds = command.EmployerDemandIds.ToList();
            for (var i = 0; i < employerDemandIds.Count; i++)
            {
                var providerInterestedEmail = new ProviderIsInterestedEmail(
                    employerDemandsFromApi[i].ContactEmailAddress, 
                    employerDemandsFromApi[i].OrganisationName,
                    employerDemandsFromApi[i].Course.Title, 
                    employerDemandsFromApi[i].Course.Level, 
                    employerDemandsFromApi[i].Location.Name, 
                    employerDemandsFromApi[i].NumberOfApprentices,
                    command.ProviderName, 
                    command.Email, 
                    command.Phone, 
                    command.Website, 
                    command.FatUrl,
                    employerDemandsFromApi[i].StopSharingUrl);
                mockApiClient.Verify(client => client.Get<GetEmployerDemandResponse>(It.Is<GetEmployerDemandRequest>(request => request.GetUrl.Contains(employerDemandIds[i].ToString()))));
                mockNotificationService.Verify(service => service.Send(It.Is<SendEmailCommand>(emailCommand => 
                    emailCommand.TemplateId == EmailConstants.ProviderInterestedTemplateId &&
                    emailCommand.RecipientsAddress == employerDemandsFromApi[i].ContactEmailAddress &&
                    emailCommand.Tokens.Count == providerInterestedEmail.Tokens.Count &&
                    !emailCommand.Tokens.Except(providerInterestedEmail.Tokens).Any())));
            }
        }

        [Test, MoqAutoData]
        public async Task And_ResponseCode_Is_Accepted_Then_Not_Send_Emails(
            CreateProviderInterestsCommand command,
            PostCreateProviderInterestsResponse responseBody,
            List<GetEmployerDemandResponse> employerDemandsFromApi,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockApiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            CreateProviderInterestsCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<PostCreateProviderInterestsResponse>(responseBody, HttpStatusCode.Accepted, null);
            mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateProviderInterestsResponse>(It.IsAny<PostCreateProviderInterestsRequest>()))
                .ReturnsAsync(apiResponse);
            mockNotificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Returns(Task.CompletedTask);
            mockApiClient
                .SetupSequence(client => client.Get<GetEmployerDemandResponse>(It.IsAny<GetEmployerDemandRequest>()))
                .ReturnsAsync(employerDemandsFromApi[0])
                .ReturnsAsync(employerDemandsFromApi[1])
                .ReturnsAsync(employerDemandsFromApi[2]);

            //Act
            var response = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            response.Should().Be(responseBody.Id);
            mockApiClient.Verify(client => client.Get<GetEmployerDemandResponse>(It.IsAny<GetEmployerDemandRequest>()), Times.Never);
            mockNotificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        }
    }
}
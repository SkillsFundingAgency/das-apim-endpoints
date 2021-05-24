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
using SFA.DAS.SharedOuterApi.Models.Messages;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.ProviderInterest.Commands
{
    public class WhenHandlingCreateProviderInterestsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Email_Sent_If_ResponseCode_Is_Created(
            CreateProviderInterestsCommand command,
            PostCreateProviderInterestsResponse responseBody,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockApiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            CreateProviderInterestsCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<PostCreateProviderInterestsResponse>(responseBody, HttpStatusCode.Created, null);
            mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateProviderInterestsResponse>(
                    It.IsAny<PostCreateProviderInterestsRequest>()))
                .ReturnsAsync(apiResponse);
            SendEmailCommand actualEmail = null;
            mockNotificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Callback((SendEmailCommand args) => actualEmail = args)
                .Returns(Task.CompletedTask);
            /*var expectedEmail = new ProviderIsInterestedEmail(
                command.,
                command.OrganisationName,
                command.CourseTitle, 
                command.CourseLevel,
                command.ConfirmationLink);*/

            //Act
            var response = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            response.Should().Be(responseBody.Id);

            foreach (var employerDemandId in command.EmployerDemandIds)
            {
                mockApiClient.Verify(client => client.Get<GetEmployerDemandResponse>(It.Is<GetEmployerDemandRequest>(request => request.GetUrl.Contains(employerDemandId.ToString()))));
                mockNotificationService.Verify(service => service.Send(It.Is<SendEmailCommand>(emailCommand => emailCommand.TemplateId == EmailConstants.ProviderInterestedTemplateId)));
            }
        }
    }
}
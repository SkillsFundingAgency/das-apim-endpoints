using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerDemand;
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
    public class WhenHandlingStopEmployerDemandCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Email_Sent_If_ResponseCode_Is_Ok(
            StopEmployerDemandCommand command,
            GetEmployerDemandResponse responseBody,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            StopEmployerDemandCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<GetEmployerDemandResponse>(responseBody, HttpStatusCode.OK, "");
            apiClient
                .Setup(x => x.PostWithResponseCode<GetEmployerDemandResponse>(It.Is<PostStopEmployerDemandRequest>(request => 
                    request.PostUrl.Contains(command.EmployerDemandId.ToString())
                )))
                .ReturnsAsync(apiResponse);

            SendEmailCommand actualEmail = null;
            mockNotificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Callback((SendEmailCommand args) => actualEmail = args)
                .Returns(Task.CompletedTask);
            var expectedEmail = new StopSharingEmployerDemandEmail(
                responseBody.ContactEmailAddress,
                responseBody.OrganisationName,
                responseBody.Course.Title,
                responseBody.Course.Level,
                responseBody.Location.Name,
                responseBody.NumberOfApprentices, 
                null);

            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(apiResponse.Body);
            actualEmail.Tokens.Should().BeEquivalentTo(expectedEmail.Tokens);
            actualEmail.RecipientsAddress.Should().BeEquivalentTo(expectedEmail.RecipientAddress);
            actualEmail.TemplateId.Should().BeEquivalentTo(expectedEmail.TemplateId);
        }
    }
}
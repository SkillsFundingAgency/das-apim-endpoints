using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.SendEmailToChangePassword;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.Users.Commands
{
    public class WhenHandlingSendEmailToChangePasswordCommand
    {
        [Test, MoqAutoData]
        public async Task Then_A_Notification_Is_Sent_And_Api_Called(
            SendEmailToChangePasswordCommand command,
            [Frozen] Mock<IOptions<ApimDeveloperMessagingConfiguration>> mockOptions,
            [Frozen] Mock<INotificationService> mockNotificationService,
            SendEmailToChangePasswordCommandHandler handler)
        {
            //Arrange
            SendEmailCommand actualEmail = null;
            mockNotificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Callback((SendEmailCommand args) => actualEmail = args)
                .Returns(Task.CompletedTask);
            var expectedEmail = new ChangePasswordEmail(command, mockOptions.Object.Value);
            
            
            //Act
            await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actualEmail.Tokens.Should().BeEquivalentTo(expectedEmail.Tokens);
            actualEmail.RecipientsAddress.Should().BeEquivalentTo(expectedEmail.RecipientAddress);
            actualEmail.TemplateId.Should().BeEquivalentTo(expectedEmail.TemplateId);
        }
    }
}
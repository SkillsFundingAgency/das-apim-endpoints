using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Api.Controllers;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.DelayedNotificationControllerTests;

public class WhenSendingOneEmail
{
    [Test, MoqAutoData]
    public async Task Then_The_Email_Should_Be_Sent(
        NotificationEmail email,
        Mock<INotificationService> notificationService,
        [Greedy] DelayedNotificationsController sut)
    {
        // arrange
        SendEmailCommand? capturedCommand = null;
        notificationService
            .Setup(x => x.Send(It.IsAny<SendEmailCommand>()))
            .Callback<SendEmailCommand>(x => capturedCommand = x);
        var expectedCommand = new SendEmailCommand(email.TemplateId.ToString(), email.RecipientAddress, email.Tokens as IReadOnlyDictionary<string, string>);    

        // act
        var result = await sut.SendOne(notificationService.Object, email) as NoContent;

        // assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        capturedCommand.Should().BeEquivalentTo(expectedCommand);
    }
}
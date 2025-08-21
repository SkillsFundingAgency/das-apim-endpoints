using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.User.Commands.UpdateUserNotificationPreferences;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ManageNotifications;

public class WhenUpdatingNotificationPreferences
{
    [Test, MoqAutoData]
    public async Task Returns_User_Not_Found(
        Guid id,
        NotificationPreferences prefs,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ManageNotificationsController sut)
    {
        // arrange
        UpdateUserNotificationPreferencesCommand? capturedCommand = null;
        mediator
            .Setup(x => x.Send(It.IsAny<UpdateUserNotificationPreferencesCommand>(), It.IsAny<CancellationToken>()))
            .Callback((IRequest<bool> x, CancellationToken _) => capturedCommand = x as UpdateUserNotificationPreferencesCommand)
            .ReturnsAsync(false);
        
        // act
        var result = await sut.PostUpdateUserNotifications(id, prefs);

        // assert
        result.Should().BeOfType<NotFoundResult>();
        capturedCommand.Should().NotBeNull();
        capturedCommand!.Id.Should().Be(id);
        capturedCommand.NotificationPreferences.Should().Be(prefs);
    }
    
    [Test, MoqAutoData]
    public async Task Returns_Notification_Preferences(
        Guid id,
        NotificationPreferences prefs,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ManageNotificationsController sut)
    {
        // arrange
        UpdateUserNotificationPreferencesCommand? capturedCommand = null;
        mediator
            .Setup(x => x.Send(It.IsAny<UpdateUserNotificationPreferencesCommand>(), It.IsAny<CancellationToken>()))
            .Callback((IRequest<bool> x, CancellationToken _) => capturedCommand = x as UpdateUserNotificationPreferencesCommand)
            .ReturnsAsync(true);
        
        // act
        var result = await sut.PostUpdateUserNotifications(id, prefs);

        // assert
        result.Should().BeOfType<OkResult>();
        capturedCommand.Should().NotBeNull();
        capturedCommand!.Id.Should().Be(id);
        capturedCommand.NotificationPreferences.Should().Be(prefs);
    }
}
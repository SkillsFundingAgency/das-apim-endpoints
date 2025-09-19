using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models.ManageUserNotificationPreferences;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByIdamsId;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ManageNotifications;

public class WhenGettingNotificationPreferences
{
    [Test, MoqAutoData]
    public async Task Returns_User_Not_Found(
        string idamsId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ManageNotificationsController sut)
    {
        // arrange
        GetUserByIdamsIdQuery? capturedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetUserByIdamsIdQuery>(), It.IsAny<CancellationToken>()))
            .Callback((IRequest<GetUserByIdamsIdQueryResult> x, CancellationToken _) => capturedQuery = x as GetUserByIdamsIdQuery)
            .ReturnsAsync(new GetUserByIdamsIdQueryResult(null));
        
        // act
        var result = await sut.GetUserNotificationPreferencesByIdams(idamsId);

        // assert
        result.Should().BeOfType<NotFoundResult>();
        capturedQuery.Should().NotBeNull();
        capturedQuery!.IdamsId.Should().Be(idamsId);
    }
    
    [Test, MoqAutoData]
    public async Task Returns_Notification_Preferences(
        string idamsId,
        GetUserResponse userResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ManageNotificationsController sut)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetUserByIdamsIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetUserByIdamsIdQueryResult(userResponse));
        
        // act
        var result = await sut.GetUserNotificationPreferencesByIdams(idamsId) as OkObjectResult;
        var value = result?.Value as GetUserNotificationPreferencesByIdamsIdResponse;

        // assert
        result.Should().NotBeNull();
        value.Should().NotBeNull();
        value!.Id.Should().Be(userResponse.Id);
        value.IdamsId.Should().Be(userResponse.IdamsUserId);
        value.NotificationPreferences.EventPreferences.Should().HaveCount(userResponse.NotificationPreferences.EventPreferences.Count);
    }
}
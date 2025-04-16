using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Settings.Requests;
using SFA.DAS.ApprenticeAan.Application.MemberNotificationSettings.Commands.UpdateNotificationSettings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MemberNotificationSettings.Commands.UpdateNotificationSettings;

public class UpdateNotificationSettingsCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        UpdateNotificationSettingsCommandHandler sut,
        UpdateNotificationSettingsCommand command,
        CancellationToken token)
    {
        // Arrange
        var expectedApiRequest = new PostNotificationSettingsApiRequest
        {
            ReceiveNotifications = command.ReceiveNotifications,
            EventTypes = command.EventTypes.Select(x => new PostNotificationSettingsApiRequest.NotificationEventType
            {
                EventType = x.EventType,
                ReceiveNotifications = x.ReceiveNotifications,
            }).ToList(),
            Locations = command.Locations.Select(x => new PostNotificationSettingsApiRequest.Location
            {
                Name = x.Name,
                Radius = x.Radius,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList(),
        };

        // Act
        await sut.Handle(command, token);

        // Assert
        apiClientMock.Verify(c => c.UpdateMemberNotificationSettings(
            command.MemberId,
            It.Is<PostNotificationSettingsApiRequest>(req =>
                req.ReceiveNotifications == expectedApiRequest.ReceiveNotifications &&
                req.EventTypes.SequenceEqual(expectedApiRequest.EventTypes, new NotificationEventTypeComparer()) &&
                req.Locations.SequenceEqual(expectedApiRequest.Locations, new LocationComparer())),
            token), Times.Once);
    }

    public class NotificationEventTypeComparer : IEqualityComparer<PostNotificationSettingsApiRequest.NotificationEventType>
    {
        public bool Equals(PostNotificationSettingsApiRequest.NotificationEventType x, PostNotificationSettingsApiRequest.NotificationEventType y)
        {
            return x.EventType == y.EventType && x.ReceiveNotifications == y.ReceiveNotifications;
        }

        public int GetHashCode(PostNotificationSettingsApiRequest.NotificationEventType obj)
        {
            return HashCode.Combine(obj.EventType, obj.ReceiveNotifications);
        }
    }

    public class LocationComparer : IEqualityComparer<PostNotificationSettingsApiRequest.Location>
    {
        public bool Equals(PostNotificationSettingsApiRequest.Location x, PostNotificationSettingsApiRequest.Location y)
        {
            return x.Name == y.Name &&
                   x.Radius == y.Radius &&
                   x.Latitude == y.Latitude &&
                   x.Longitude == y.Longitude;
        }

        public int GetHashCode(PostNotificationSettingsApiRequest.Location obj)
        {
            return HashCode.Combine(obj.Name, obj.Radius, obj.Latitude, obj.Longitude);
        }
    }

}

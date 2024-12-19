using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.EmployerAan.Application.Settings.Commands;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Settings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Settings.NotificationLocations
{
    [TestFixture]
    public class UpdateNotificationSettingsCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_InvokesApiClientWithCorrectParameters(
            [Frozen] Mock<IAanHubRestApiClient> aanHubApiClientMock,
            UpdateNotificationSettingsCommand command,
            UpdateNotificationSettingsCommandHandler handler,
            CancellationToken cancellationToken)
        {
            // Act
            await handler.Handle(command, cancellationToken);

            // Assert
            aanHubApiClientMock.Verify(client => client.UpdateMemberNotificationLocations(
                command.MemberId,
                It.Is<NotificationLocationsPostApiRequest>(request =>
                    request.Locations.Count == command.Locations.Count &&
                    request.ReceiveNotifications == command.ReceiveNotifications &&
                    request.EventTypes.All(et => command.EventTypes.Any(cmdEt => 
                        cmdEt.EventType == et.EventType &&
                        cmdEt.ReceiveNotifications == et.ReceiveNotifications &&
                        cmdEt.Ordering == et.Ordering
                        )) &&
                    request.Locations.All(loc => command.Locations.Any(cmdLoc =>
                        cmdLoc.Name == loc.Name &&
                        cmdLoc.Radius == loc.Radius &&
                        cmdLoc.Latitude == loc.Latitude &&
                        cmdLoc.Longitude == loc.Longitude))),
                cancellationToken), Times.Once);
        }
    }
}

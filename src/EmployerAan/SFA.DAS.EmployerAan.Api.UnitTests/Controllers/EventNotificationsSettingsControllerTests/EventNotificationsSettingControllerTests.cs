using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Settings.Commands;
using SFA.DAS.EmployerAan.Models.ApiRequests.Settings;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.EventNotificationsSettingsControllerTests
{
    public class EventNotificationsSettingsControllerTests
    {
        [Test, MoqAutoData]
        public async Task Get_ReturnsCorrectViewModel(
            GetNotificationsLocationsQueryResult response,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EventNotificationsSettingsController sut)
        {
            mockMediator
                .Setup(m => m.Send(It.IsAny<GetNotificationsLocationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await sut.Get(12345, "exampleSearchTerm", Guid.Empty);

            result.As<OkObjectResult>().Should().NotBeNull();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
        }

        [Test, MoqAutoData]
        public async Task Post_InvokesCommandHandlerWithCorrectParameters(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] MemberNotificationSettingsController sut,
            Guid memberId,
            NotificationsSettingsApiRequest request,
            CancellationToken cancellationToken)
        {
            // Act
            var result = await sut.Post(memberId, request);

            // Assert
            mockMediator.Verify(m => m.Send(It.Is<UpdateNotificationSettingsCommand>(cmd =>
                cmd.MemberId == memberId &&
                cmd.ReceiveNotifications == request.ReceiveNotifications &&
                cmd.EventTypes.All(l => request.EventTypes.Any(reqEventType => 
                    reqEventType.EventType == l.EventType &&
                    reqEventType.ReceiveNotifications == l.ReceiveNotifications)
                ) &&
                cmd.Locations.All(l => request.Locations.Any(reqLoc =>
                    reqLoc.Name == l.Name &&
                    reqLoc.Radius == l.Radius &&
                    reqLoc.Latitude == l.Latitude &&
                    reqLoc.Longitude == l.Longitude))), It.IsAny<CancellationToken>()), Times.Once);

            result.Should().BeOfType<OkResult>();
        }
    }
}

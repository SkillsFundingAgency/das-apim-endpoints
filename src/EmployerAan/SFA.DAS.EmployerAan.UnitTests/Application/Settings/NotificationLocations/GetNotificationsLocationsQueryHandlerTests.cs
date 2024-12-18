using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.Application.MemberNotificationLocations.Queries;
using SFA.DAS.EmployerAan.Application.MemberNotificationEventFormat.Queries.GetMemberNotificationEventFormats;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Settings.NotificationLocations
{
    [TestFixture]
    public class GetNotificationsLocationsQueryHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_Return_Matches_From_Search_When_LocationLookupService_Returns_Nothing(
            GetNotificationsLocationsQuery query,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
            [Frozen] GetNotificationsLocationsQueryHandler handler,
            GetLocationsListResponse apiResponse)
        {
            var expectedApiRequest = new GetLocationsQueryRequest(query.SearchTerm);

            mockLocationLookupService
                .Setup(x => x.GetLocationInformation(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<bool>()))
                .ReturnsAsync(() => null);

            apiClient
                .Setup(x => x.Get<GetLocationsListResponse>(It.Is<GetLocationsQueryRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Locations.Should().BeEquivalentTo(apiResponse.Locations.Select(x => new GetNotificationsLocationsQueryResult.Location
            {
                Name = x.DisplayName,
                GeoPoint = x.Location.GeoPoint
            }).Take(GetNotificationsLocationsQueryHandler.MaxResults));
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_Return_A_Single_Match_From_LocationLookupService(
            GetNotificationsLocationsQuery query,
            LocationItem locationData,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            [Frozen] GetNotificationsLocationsQueryHandler handler)
        {
            apiClient
                .Setup(x => x.Get<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
                .ReturnsAsync(new GetLocationsListResponse { Locations = new List<GetLocationsListItem>() });

            mockLocationLookupService
                .Setup(x => x.GetLocationInformation(query.SearchTerm, 0, 0, false))
                .ReturnsAsync(locationData);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Locations.Should().BeEquivalentTo(new List<GetNotificationsLocationsQueryResult.Location>
            {
                new()
                {
                    Name = locationData.Name,
                    GeoPoint = locationData.GeoPoint
                }
            });
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_Return_Current_Settings_When_SearchTerm_Is_Empty(
            GetNotificationsLocationsQuery query,
            [Frozen] Mock<IAanHubRestApiClient> mockAanHubApiClient,
            [Frozen] GetNotificationsLocationsQueryHandler handler,
            GetMemberNotificationLocationsQueryResult locationsResponse,
            GetMemberNotificationEventFormatsQueryResult eventFormatsResponse)
        {
            query.SearchTerm = string.Empty;

            mockAanHubApiClient
                .Setup(x => x.GetMemberNotificationLocations(query.MemberId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(locationsResponse);

            mockAanHubApiClient
                .Setup(x => x.GetMemberNotificationEventFormat(query.MemberId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventFormatsResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.SavedLocations.Should().BeEquivalentTo(locationsResponse.MemberNotificationLocations.Select(x => new GetNotificationsLocationsQueryResult.AddedLocation
            {
                Name = x.Name,
                Radius = x.Radius,
                Coordinates = new[] { x.Latitude, x.Longitude }
            }).ToList());

            result.NotificationEventTypes.Should().BeEquivalentTo(eventFormatsResponse.MemberNotificationEventFormats.Select(x => new GetNotificationsLocationsQueryResult.NotificationEventType
            {
                EventFormat = x.EventFormat,
                Ordering = x.Ordering,
                ReceiveNotifications = x.ReceiveNotifications
            }).ToList());
        }
    }
}

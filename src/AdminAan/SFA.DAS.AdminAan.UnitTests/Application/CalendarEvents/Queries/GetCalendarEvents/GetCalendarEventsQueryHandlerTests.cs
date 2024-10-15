using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventsQueryHandler handler,
        GetCalendarEventsQueryResult expected,
        Guid requestedByMemberId,
        bool? isActive,
        DateTime? fromDate,
        DateTime? toDate,
        List<int> calendarIds,
        List<int> regionIds,
        int? page,
        int? pageSize,
        bool showUserEventsOnly,
        CancellationToken cancellationToken)
    {
        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = requestedByMemberId,
            IsActive = isActive,
            FromDate = fromDate?.ToString("yyyy-MM-dd"),
            ToDate = toDate?.ToString("yyyy-MM-dd"),
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Page = page,
            PageSize = pageSize,
            ShowUserEventsOnly = showUserEventsOnly
        };

        apiClient.Setup(x => x.GetCalendarEvents(requestedByMemberId, It.IsAny<Dictionary<string, string[]>>(), cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_When_Invalid_Location_Is_Provided_Then_Search_Using_Its_Geopoint(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        [Frozen] Mock<ILocationLookupService> locationLookupService,
        GetCalendarEventsQuery query,
        GetCalendarEventsQueryHandler handler)
    {
        locationLookupService.Setup(x => x.GetLocationInformation(query.Location, 0, 0, false))
            .ReturnsAsync(() => null);

        var result = await handler.Handle(query, default);

        result.IsInvalidLocation.Should().BeTrue();

        apiClient.Verify(x => x.GetCalendarEvents(It.IsAny<Guid>(), It.IsAny<IDictionary<string, string[]>>(), It.IsAny<CancellationToken>()), Times.Never);

    }
}

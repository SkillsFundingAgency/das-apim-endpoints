using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.CalendarEvents;
using SFA.DAS.EmployerAan.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventsQueryHandler handler,
        GetCalendarEventsApiResponse apiResponse,
        Guid requestedByMemberId,
        string keyword,
        DateTime? fromDate,
        DateTime? toDate,
        List<EventFormat> eventFormats,
        List<int> calendarIds,
        List<int> regionIds,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = requestedByMemberId,
            Keyword = keyword,
            FromDate = fromDate?.ToString("yyyy-MM-dd"),
            ToDate = toDate?.ToString("yyyy-MM-dd"),
            EventFormat = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Page = page,
            PageSize = pageSize

        };

        apiClient.Setup(x => x.GetCalendarEvents(requestedByMemberId, It.IsAny<Dictionary<string, string[]>>(), cancellationToken)).ReturnsAsync(apiResponse);
        var actual = await handler.Handle(query, cancellationToken);
        Assert.That(actual, Is.Not.Null);
        actual.CalendarEvents.Should().BeEquivalentTo(apiResponse.CalendarEvents, config => config.ExcludingMissingMembers());
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

    [Test, MoqAutoData]
    public async Task Handle_Returns_Regions(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheService,
        GetCalendarEventsQueryHandler handler,
        GetRegionsQueryResult apiResponse,
        GetCalendarEventsQuery query,
        CancellationToken cancellationToken)
    {
        cacheService.Setup(x => x.RetrieveFromCache<List<GetCalendarEventsQueryResult.RegionData>>(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        apiClient.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(apiResponse);
        var actual = await handler.Handle(query, cancellationToken);

        actual.Regions.Should().BeEquivalentTo(apiResponse.Regions, config => config.ExcludingMissingMembers());
    }

    [Test, MoqAutoData]
    public async Task Handle_Returns_Calendars(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheService,
        GetCalendarEventsQueryHandler handler,
        List<Calendar> apiResponse,
        GetCalendarEventsQuery query,
        CancellationToken cancellationToken)
    {
        cacheService.Setup(x => x.RetrieveFromCache<List<GetCalendarEventsQueryResult.CalendarType>>(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        apiClient.Setup(x => x.GetCalendars(cancellationToken)).ReturnsAsync(apiResponse);
        var actual = await handler.Handle(query, cancellationToken);

        actual.Calendars.Should().BeEquivalentTo(apiResponse, config => config.ExcludingMissingMembers());
    }
}

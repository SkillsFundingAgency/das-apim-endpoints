using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventsQueryHandler handler,
        GetCalendarEventsQueryResult expected,
        Guid requestedByMemberId,
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
            FromDate = fromDate?.ToString("yyyy-MM-dd"),
            ToDate = toDate?.ToString("yyyy-MM-dd"),
            EventFormat = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Page = page,
            PageSize = pageSize

        };
        apiClient.Setup(x => x.GetCalendarEvents(requestedByMemberId.ToString(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<EventFormat>>(), It.IsAny<List<int>>(), It.IsAny<List<int>>(), It.IsAny<int?>(), It.IsAny<int?>(), cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
    }
}
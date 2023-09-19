using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventsQueryHandler handler,
        GetCalendarEventsQueryResult expected,
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

        apiClient.Setup(x => x.GetCalendarEvents(requestedByMemberId, It.IsAny<Dictionary<string, string[]>>(), cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
    }
}

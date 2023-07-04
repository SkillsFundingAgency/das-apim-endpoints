using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

public class QueryStringParameterBuilderTests
{

    [Test, AutoData]
    public void Operator_PopulatesModelFromParameters(Guid memberId, DateTime? fromDate, DateTime? toDate,
        List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds, int? page, int? pageSize)
    {
        var model = new GetCalendarEventsModel
        {
            RequestedByMemberId = memberId,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };

        var query = (GetCalendarEventsQuery)model;

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(query);

        parameters.TryGetValue("fromDate", out string[]? fromDateResult);
        fromDateResult![0].Should().Be(fromDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("toDate", out var toDateResult);
        toDateResult![0].Should().Be(toDate?.ToString("yyyy-MM-dd"));

        parameters.TryGetValue("eventFormat", out var eventFormatResult);
        eventFormatResult!.Length.Should().Be(eventFormats.Count);
        eventFormats.Select(x => x.ToString()).Should().BeEquivalentTo(eventFormatResult.ToList());

        parameters.TryGetValue("calendarId", out var calendarIdsResult);
        calendarIdsResult!.Length.Should().Be(calendarIds.Count);
        calendarIds.Select(x => x.ToString()).Should().BeEquivalentTo(calendarIdsResult.ToList());

        parameters.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());

        parameters.TryGetValue("page", out string[]? pageResult);
        pageResult![0].Should().Be(page?.ToString());

        parameters.TryGetValue("pageSize", out string[]? pageSizeResult);
        pageSizeResult![0].Should().Be(pageSize?.ToString());
    }
}
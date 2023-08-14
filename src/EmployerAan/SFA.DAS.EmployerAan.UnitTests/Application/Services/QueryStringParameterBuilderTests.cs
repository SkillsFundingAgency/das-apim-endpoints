using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Application.Models;
using SFA.DAS.EmployerAan.Application.Services;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Services;
public class QueryStringParameterBuilderTests
{

    [Test, AutoData]
    public void Operator_PopulatesModelFromParameters(Guid memberId, string keyword, DateTime? fromDate, DateTime? toDate,
        List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds, int? page, int? pageSize)
    {
        var sut = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = memberId,
            Keyword = keyword,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };

        var query = (GetCalendarEventsQuery)sut;

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(query);

        parameters.TryGetValue("keyword", out string[]? keywordResult);
        keywordResult![0].Should().Be(keyword);

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

        parameters.TryGetValue("isActive", out string[]? isActiveResult);
        isActiveResult![0].Should().Be("true");
    }

    [TestCase(null)]
    [TestCase("2030-06-01")]
    public void Builder_ConstructParameters_FromDate(DateTime? fromDate)
    {
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = Guid.NewGuid(),
            FromDate = fromDate
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("fromDate", out var fromDateResult);
        if (fromDate != null)
        {
            fromDateResult![0].Should().Be(fromDate?.ToString("yyyy-MM-dd"));
        }
        else
        {
            fromDateResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase("2030-06-01")]
    public void Builder_ConstructParameters_ToDate(DateTime? toDate)
    {
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = Guid.NewGuid(),
            ToDate = toDate
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);


        parameters.TryGetValue("toDate", out var toDateResult);
        if (toDate != null)
        {
            toDateResult![0].Should().Be(toDate?.ToString("yyyy-MM-dd"));
        }
        else
        {
            toDateResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(EventFormat.Hybrid)]
    public void Builder_ConstructParameters_EventFormats(EventFormat? eventFormat)
    {
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = Guid.NewGuid(),
        };

        if (eventFormat != null)
        {
            model.EventFormat = new List<EventFormat> { eventFormat.Value };
        }

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("eventFormat", out var eventFormatResult);
        if (eventFormatResult != null)
        {
            eventFormatResult![0].Should().BeEquivalentTo(model.EventFormat[0].ToString());
        }
        else
        {
            eventFormatResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(1)]
    public void Builder_ConstructParameters_CalendarIds(int? calendarId)
    {
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = Guid.NewGuid(),
        };

        if (calendarId != null)
        {
            model.CalendarId = new List<int> { calendarId.Value };
        }

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("calendarId", out var calendarIdResult);
        if (calendarIdResult != null)
        {
            calendarIdResult![0].Should().BeEquivalentTo(model.CalendarId[0].ToString());
        }
        else
        {
            calendarIdResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(1)]
    public void Builder_ConstructParameters_RegionIds(int? regionId)
    {
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = Guid.NewGuid(),
        };

        if (regionId != null)
        {
            model.RegionId = new List<int> { regionId.Value };
        }

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("regionId", out var regionIdResult);
        if (regionIdResult != null)
        {
            regionIdResult![0].Should().BeEquivalentTo(model.RegionId[0].ToString());
        }
        else
        {
            regionIdResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(3)]
    public void Builder_ConstructParameters_ToPage(int? page)
    {
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = Guid.NewGuid(),
            Page = page
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("page", out var pageResult);
        if (pageResult != null)
        {
            pageResult![0].Should().Be(page?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(6)]
    public void Builder_ConstructParameters_ToPageSize(int? pageSize)
    {
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = Guid.NewGuid(),
            PageSize = pageSize
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("pageSize", out var pageResult);
        if (pageResult != null)
        {
            pageResult![0].Should().Be(pageSize?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }
}

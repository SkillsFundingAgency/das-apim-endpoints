using FluentAssertions;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AdminAan.Services;

namespace SFA.DAS.AdminAan.UnitTests.Services;
public class QueryStringParameterBuilderTests
{
    [TestCase(null)]
    [TestCase("2030-06-01")]
    public void Builder_ConstructParameters_FromDate(string? fromDate)
    {
        var model = new GetCalendarEventsQuery
        {
            RequestedByMemberId = Guid.NewGuid(),
            FromDate = fromDate
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("fromDate", out var fromDateResult);
        if (fromDate != null)
        {
            fromDateResult![0].Should().Be(fromDate);
        }
        else
        {
            fromDateResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase("2030-06-01")]
    public void Builder_ConstructParameters_ToDate(string? toDate)
    {
        var model = new GetCalendarEventsQuery
        {
            RequestedByMemberId = Guid.NewGuid(),
            ToDate = toDate
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);


        parameters.TryGetValue("toDate", out var toDateResult);
        if (toDate != null)
        {
            toDateResult![0].Should().Be(toDate);
        }
        else
        {
            toDateResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(true)]
    [TestCase(false)]
    public void Builder_ConstructParameters_IsActive(bool? isActive)
    {
        var model = new GetCalendarEventsQuery
        {
            RequestedByMemberId = Guid.NewGuid(),
        };

        if (isActive != null)
        {
            model.IsActive = isActive;
        }

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("isActive", out var isActiveResult);
        if (isActiveResult != null)
        {
            isActiveResult![0].Should().BeEquivalentTo(model.IsActive.ToString());
        }
        else
        {
            isActiveResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(1)]
    public void Builder_ConstructParameters_CalendarIds(int? calendarId)
    {
        var model = new GetCalendarEventsQuery
        {
            RequestedByMemberId = Guid.NewGuid(),
        };

        if (calendarId != null)
        {
            model.CalendarIds = new List<int> { calendarId.Value };
        }

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("calendarId", out var calendarIdResult);
        if (calendarIdResult != null)
        {
            calendarIdResult![0].Should().BeEquivalentTo(model.CalendarIds![0].ToString());
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
        var model = new GetCalendarEventsQuery
        {
            RequestedByMemberId = Guid.NewGuid(),
        };

        if (regionId != null)
        {
            model.RegionIds = new List<int> { regionId.Value };
        }

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("regionId", out var regionIdResult);
        if (regionIdResult != null)
        {
            regionIdResult![0].Should().BeEquivalentTo(model.RegionIds![0].ToString());
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
        var model = new GetCalendarEventsQuery
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
        var model = new GetCalendarEventsQuery
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

using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Models;
public class GetCalendarEventsRequestModelTests
{

    [Test, AutoData]
    public void Operator_PopulatesQueryFromModel(GetCalendarEventsRequestModel model)
    {
        var query = (GetCalendarEventsQuery)model;

        query.RequestedByMemberId.Should().Be(model.RequestedByMemberId);
        query.Keyword.Should().Be(model.Keyword);
        query.FromDate.Should().Be(model.FromDate?.ToString("yyyy-MM-dd"));
        query.ToDate.Should().Be(model.ToDate?.ToString("yyyy-MM-dd"));
        query.EventFormat.Should().BeEquivalentTo(model.EventFormat);
        query.Page.Should().Be(model.Page);
        query.PageSize.Should().Be(model.PageSize);
    }

    [Test, AutoData]
    public void Operator_PopulatesQueryFromModelWithNulls(Guid memberId)
    {
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = memberId,
        };

        var query = (GetCalendarEventsQuery)model;

        query.RequestedByMemberId.Should().Be(memberId);
        query.Keyword.Should().BeEmpty();
        query.FromDate.Should().BeNull();
        query.ToDate.Should().BeNull();
        query.Page.Should().BeNull();
        query.PageSize.Should().BeNull();
    }
}
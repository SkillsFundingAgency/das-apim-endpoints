using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Application.Schools.Queries;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.UnitTests.Application.CalendarEvents.Queries.GetCalendarEvent;
public class GetCalendarEventQueryHandlerTests
{
    private const int RegionId = 1;
    private const string RegionName = "West";
    private const long Urn = 123456;
    private const string SchoolName = "School name";

    [TestCase(null, null, null, null)]
    [TestCase(RegionId, RegionName, null, null)]
    [TestCase(null, null, Urn, SchoolName)]
    [TestCase(RegionId, RegionName, Urn, SchoolName)]
    public async Task Handle_ReturnCalendarEvents(int? regionId, string? regionName, long? urn, string? schoolName)
    {
        var cancellationToken = new CancellationToken();
        var aanHubRestApiClientMock = new Mock<IAanHubRestApiClient>();
        var referenceDataApiClient = new Mock<IReferenceDataApiClient>();
        var expected = new GetCalendarEventQueryResult();
        var requestedByMemberId = Guid.NewGuid();
        var calendarEventId = Guid.NewGuid();
        var regions = new List<Region>
        {
            new(RegionId, RegionName, 1),
            new(2, "East", 2)
        };
        var regionsResult = new GetRegionsQueryResult(regions);

        expected.CalendarEventId = calendarEventId;
        expected.RegionId = regionId;
        expected.Urn = urn;

        aanHubRestApiClientMock.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(regionsResult);
        aanHubRestApiClientMock.Setup(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken)).ReturnsAsync(expected);

        if (urn.HasValue)
        {
            var schoolResult = new GetSchoolApiResult(schoolName!);

            var response = new Response<GetSchoolApiResult>(
                "not used",
                new HttpResponseMessage(System.Net.HttpStatusCode.OK),
                () => schoolResult);

            referenceDataApiClient.Setup(x => x.GetSchoolFromUrn(urn.Value.ToString(), 4, cancellationToken)).ReturnsAsync(response);
        }

        var handler = new GetCalendarEventQueryHandler(aanHubRestApiClientMock.Object, referenceDataApiClient.Object);
        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
        actual?.RegionName.Should().Be(regionName);
        actual?.SchoolName.Should().Be(schoolName);
    }
}

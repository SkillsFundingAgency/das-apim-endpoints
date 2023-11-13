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

    [TestCase(null, null, null, null, false)]
    [TestCase(RegionId, RegionName, null, null, false)]
    [TestCase(null, null, Urn, SchoolName, false)]
    [TestCase(RegionId, RegionName, Urn, SchoolName, false)]
    [TestCase(RegionId, RegionName, Urn, null, true)]
    public async Task Handle_ReturnCalendarEvents(int? regionId, string? regionName, long? urn, string? schoolName,
        bool schoolApiReturnsBadRequest)
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
        aanHubRestApiClientMock.Setup(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken))
            .ReturnsAsync(expected);

        if (urn.HasValue)
        {
            var schoolResult = new GetSchoolApiResult(schoolName!);

            var status = System.Net.HttpStatusCode.OK;
            if (schoolApiReturnsBadRequest)
            {
                status = System.Net.HttpStatusCode.BadRequest;
            }

            var response = new Response<GetSchoolApiResult>(
                "not used",
                new HttpResponseMessage(status),
                () => schoolResult);

            referenceDataApiClient.Setup(x => x.GetSchoolFromUrn(urn.Value.ToString(), 4, cancellationToken))
                .ReturnsAsync(response);
        }

        var handler = new GetCalendarEventQueryHandler(aanHubRestApiClientMock.Object, referenceDataApiClient.Object);
        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
        actual?.RegionName.Should().Be(regionName);
        actual?.SchoolName.Should().Be(schoolName);
    }

    [Test]
    public async Task Handle_APIReturnsNullResult()
    {
        var aanHubRestApiClientMock = new Mock<IAanHubRestApiClient>();
        var referenceDataApiClient = new Mock<IReferenceDataApiClient>();
        var expected = new GetCalendarEventQueryResult();
        var requestedByMemberId = Guid.NewGuid();
        var calendarEventId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        aanHubRestApiClientMock.Setup(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken))
            .ReturnsAsync((GetCalendarEventQueryResult?)null);

        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);
        var handler = new GetCalendarEventQueryHandler(aanHubRestApiClientMock.Object, referenceDataApiClient.Object);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(null);
        aanHubRestApiClientMock.Verify(a => a.GetRegions(It.IsAny<CancellationToken>()), Times.Never);
        referenceDataApiClient.Verify(
            r => r.GetSchoolFromUrn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Handle_ReturnCalendarEventSchoolApiThrowsException()
    {
        var regionId = 1;
        var urn = 123;
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

        referenceDataApiClient.Setup(x => x.GetSchoolFromUrn(urn.ToString(), 4, cancellationToken))
            .ThrowsAsync(new SystemException());

        var handler = new GetCalendarEventQueryHandler(aanHubRestApiClientMock.Object, referenceDataApiClient.Object);
        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
        actual?.RegionName.Should().Be(RegionName);
        actual?.SchoolName.Should().BeNull();
        referenceDataApiClient.Verify(
            r => r.GetSchoolFromUrn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);

    }
}

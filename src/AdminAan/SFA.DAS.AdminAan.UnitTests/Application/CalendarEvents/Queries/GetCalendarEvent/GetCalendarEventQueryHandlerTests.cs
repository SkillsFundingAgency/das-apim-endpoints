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

    [TestCase(null, null)]
    [TestCase(RegionId, RegionName)]
    public async Task Handle_ReturnCalendarEvents_WithRegionName(int? regionId, string? regionName)
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


        aanHubRestApiClientMock.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(regionsResult);
        aanHubRestApiClientMock.Setup(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken))
            .ReturnsAsync(expected);

        var handler = new GetCalendarEventQueryHandler(aanHubRestApiClientMock.Object, referenceDataApiClient.Object);
        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
        actual?.RegionName.Should().Be(regionName);
        var callCount = regionId == null ? 0 : 1;
        aanHubRestApiClientMock.Verify(x => x.GetRegions(cancellationToken), Times.Exactly(callCount));
        aanHubRestApiClientMock.Verify(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken), Times.Once);
        referenceDataApiClient.Verify(x => x.GetSchoolFromUrn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestCase(null, null, false)]
    [TestCase(Urn, SchoolName, false)]
    [TestCase(Urn, null, true)]
    public async Task Handle_ReturnCalendarEvents_WithSchoolName(long? urn, string? schoolName, bool schoolApiReturnsBadRequest)
    {
        const int OrganisationEducationType = 4;
        var cancellationToken = new CancellationToken();
        var aanHubRestApiClientMock = new Mock<IAanHubRestApiClient>();
        var referenceDataApiClient = new Mock<IReferenceDataApiClient>();
        var expected = new GetCalendarEventQueryResult();
        var requestedByMemberId = Guid.NewGuid();
        var calendarEventId = Guid.NewGuid();

        expected.CalendarEventId = calendarEventId;
        expected.Urn = urn;

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

            referenceDataApiClient.Setup(x => x.GetSchoolFromUrn(urn.Value.ToString(), OrganisationEducationType, cancellationToken))
                .ReturnsAsync(response);
        }

        var handler = new GetCalendarEventQueryHandler(aanHubRestApiClientMock.Object, referenceDataApiClient.Object);
        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
        actual?.SchoolName.Should().Be(schoolName);
        aanHubRestApiClientMock.Verify(x => x.GetRegions(cancellationToken), Times.Never);
        aanHubRestApiClientMock.Verify(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken), Times.Once);
        var callCount = urn == null ? 0 : 1;
        referenceDataApiClient.Verify(x => x.GetSchoolFromUrn(It.IsAny<string>(), OrganisationEducationType, cancellationToken), Times.Exactly(callCount));
    }
}

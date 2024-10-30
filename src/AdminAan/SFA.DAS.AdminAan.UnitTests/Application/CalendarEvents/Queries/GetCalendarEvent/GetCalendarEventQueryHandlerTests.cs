using AutoFixture;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Application.Schools.Queries;
using SFA.DAS.AdminAan.Domain.InnerApi.AanHubApi.Responses;
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
        var fixture = new Fixture();

        var cancellationToken = new CancellationToken();
        var aanHubRestApiClientMock = new Mock<IAanHubRestApiClient>();
        var referenceDataApiClient = new Mock<IReferenceDataApiClient>();
        var apiResponse = fixture.Create<GetCalendarEventByIdApiResponse>();
        var requestedByMemberId = Guid.NewGuid();
        var calendarEventId = Guid.NewGuid();
        var regions = new List<Region>
        {
            new(RegionId, RegionName, 1),
            new(2, "East", 2)
        };
        var regionsResult = new GetRegionsQueryResult(regions);

        apiResponse.CalendarEventId = calendarEventId;
        apiResponse.RegionId = regionId;
        apiResponse.Urn = null;

        aanHubRestApiClientMock.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(regionsResult);
        aanHubRestApiClientMock.Setup(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken))
            .ReturnsAsync(apiResponse);

        var handler = new GetCalendarEventQueryHandler(aanHubRestApiClientMock.Object, referenceDataApiClient.Object);
        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
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
        var fixture = new Fixture();

        const int OrganisationEducationType = 4;
        var cancellationToken = new CancellationToken();
        var aanHubRestApiClientMock = new Mock<IAanHubRestApiClient>();
        var apiResponse = fixture.Create<GetCalendarEventByIdApiResponse>();
        var referenceDataApiClient = new Mock<IReferenceDataApiClient>();
        var requestedByMemberId = Guid.NewGuid();
        var calendarEventId = Guid.NewGuid();

        apiResponse.CalendarEventId = calendarEventId;
        apiResponse.Urn = urn;
        apiResponse.RegionId = null;

        aanHubRestApiClientMock.Setup(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken))
            .ReturnsAsync(apiResponse);

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
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        actual?.SchoolName.Should().Be(schoolName);
        aanHubRestApiClientMock.Verify(x => x.GetRegions(cancellationToken), Times.Never);
        aanHubRestApiClientMock.Verify(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken), Times.Once);
        var callCount = urn == null ? 0 : 1;
        referenceDataApiClient.Verify(x => x.GetSchoolFromUrn(It.IsAny<string>(), OrganisationEducationType, cancellationToken), Times.Exactly(callCount));
    }
}

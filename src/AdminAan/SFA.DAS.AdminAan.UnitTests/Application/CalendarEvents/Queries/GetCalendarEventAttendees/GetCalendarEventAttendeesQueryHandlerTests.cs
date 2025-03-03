using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEventAttendees;
using SFA.DAS.AdminAan.Domain.InnerApi.AanHubApi.Responses;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.CalendarEvents.Queries.GetCalendarEventAttendees;

public class GetCalendarEventQueryAttendeesHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnCalendarEventAttendees(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        GetCalendarEventByIdApiResponse apiResponse,
        GetCalendarEventAttendeesQueryHandler handler,
        Guid requestedByMemberId,
        Guid calendarEventId,
        CancellationToken cancellationToken)
    {
        apiResponse.CalendarEventId = calendarEventId;
        aanHubRestApiClientMock.Setup(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken))
            .ReturnsAsync(apiResponse);

        var query = new GetCalendarEventAttendeesQuery(requestedByMemberId, calendarEventId);

        var actual = await handler.Handle(query, cancellationToken);

        actual.Attendees.Should().BeEquivalentTo(apiResponse.Attendees.Select(x => new
        {
            Name = x.MemberName,
            SignUpDate = x.AddedDate,
            x.Email
        }));
    }
}

using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEventAttendees;

public class GetCalendarEventAttendeesQueryHandler(IAanHubRestApiClient apiClient) : IRequestHandler<GetCalendarEventAttendeesQuery, GetCalendarEventAttendeesQueryResult>
{
    public async Task<GetCalendarEventAttendeesQueryResult> Handle(GetCalendarEventAttendeesQuery request,
        CancellationToken cancellationToken)
    {
        var apiResponse = await apiClient.GetCalendarEvent(request.RequestedByMemberId, request.CalendarEventId,
            cancellationToken);

        return new GetCalendarEventAttendeesQueryResult
        {
            Attendees = apiResponse.Attendees.Select(x => new GetCalendarEventAttendeesQueryResult.Attendee
            {
                Name = x.MemberName,
                Email = x.Email,
                SignUpDate = x.AddedDate!.Value
            }).ToList()
        };
    }
}
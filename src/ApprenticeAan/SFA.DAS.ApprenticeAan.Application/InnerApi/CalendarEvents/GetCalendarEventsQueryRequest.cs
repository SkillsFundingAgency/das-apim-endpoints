using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.CalendarEvents;

public record GetCalendarEventsQueryRequest : IGetApiRequest
{
    public string GetUrl => $"calendarEvents";
}

using SFA.DAS.AdminAan.Domain;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
public record PutEventGuestsModel(List<GuestSpeaker> Guests);


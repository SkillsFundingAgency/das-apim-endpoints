using MediatR;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Update;

public class PutEventCommandHandler : IRequestHandler<PutEventCommand, Unit>
{
    private readonly IAanHubRestApiClient _apiClient;

    public PutEventCommandHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Unit> Handle(PutEventCommand command, CancellationToken cancellationToken)
    {
        var response = await _apiClient.PutCalendarEvent(command.RequestedByMemberId!, command.CalendarEventId!, command, cancellationToken);

        if (response.ResponseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            if (command.Guests.Any())
            {
                var guestsList = new PutEventGuestsModel(command.Guests);
                await _apiClient.PutGuestSpeakers(command.CalendarEventId, command.RequestedByMemberId!, guestsList, cancellationToken);
            }

            return Unit.Value;
        }

        throw new InvalidOperationException($"An attempt to update a calendarEvent id: {command.CalendarEventId} by member id {command.RequestedByMemberId}, came back with unsuccessful response status: {response.ResponseMessage.StatusCode} with message: {response.StringContent}");
    }
}
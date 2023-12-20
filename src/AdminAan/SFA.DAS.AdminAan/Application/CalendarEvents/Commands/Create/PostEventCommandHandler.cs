using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;

public class PostEventCommandHandler : IRequestHandler<PostEventCommand, PostEventCommandResult>
{
    private readonly IAanHubRestApiClient _apiClient;

    public PostEventCommandHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PostEventCommandResult> Handle(PostEventCommand command, CancellationToken cancellationToken)
    {
        var postEventResult = await _apiClient.PostCalendarEvents(command.RequestedByMemberId!, command, cancellationToken);

        var eventId = postEventResult.CalendarEventId;

        if (command.Guests.Any())
        {
            var guestsList = new PutEventGuestsModel(command.Guests);
            await _apiClient.PutGuestSpeakers(eventId, command.RequestedByMemberId!, guestsList, cancellationToken);
        }

        return postEventResult;
    }
}

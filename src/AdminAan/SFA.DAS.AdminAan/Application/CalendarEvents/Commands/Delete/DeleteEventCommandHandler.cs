using MediatR;
using SFA.DAS.AdminAan.Infrastructure;
using System.Net;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Delete;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, Unit>
{
    private readonly IAanHubRestApiClient _apiClient;

    public DeleteEventCommandHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Unit> Handle(DeleteEventCommand command, CancellationToken cancellationToken)
    {
        var response = await _apiClient.DeleteCalendarEvent(command.RequestedByMemberId!, command.CalendarEventId!, cancellationToken);
        if (response.ResponseMessage.StatusCode == HttpStatusCode.NoContent) return Unit.Value;

        throw new InvalidOperationException($"An attempt to delete a calendarEvent id: {command.CalendarEventId} by member id {command.RequestedByMemberId}, came back with unsuccessful response status: {response.ResponseMessage.StatusCode} with message: {response.StringContent}");
    }
}
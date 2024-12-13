using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.MemberNotificationEventFormat.Queries.GetMemberNotificationEventFormats;

public class GetMemberNotificationEventFormatsQueryHandler : IRequestHandler<GetMemberNotificationEventFormatsQuery, GetMemberNotificationEventFormatsQueryResult>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetMemberNotificationEventFormatsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetMemberNotificationEventFormatsQueryResult> Handle(GetMemberNotificationEventFormatsQuery request, CancellationToken cancellationToken)
    {
        var eventFormats = await _apiClient.GetMemberNotificationEventFormat(request.MemberId, cancellationToken);

        return eventFormats;
    }
}

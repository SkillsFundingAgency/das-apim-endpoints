using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.NotificationsLocations.Queries;
public class GetNotificationsLocationsQuery : IRequest<GetNotificationsLocationsQueryResult>
{
    public string SearchTerm { get; set; }
}
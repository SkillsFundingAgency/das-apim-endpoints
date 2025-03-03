using MediatR;

namespace SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations
{
    public class GetNotificationsLocationsQuery : IRequest<GetNotificationsLocationsQueryResult>
    {
        public long EmployerAccountId { get; set; }
        public Guid MemberId { get; set; }
        public string? SearchTerm { get; set; }
    }
}

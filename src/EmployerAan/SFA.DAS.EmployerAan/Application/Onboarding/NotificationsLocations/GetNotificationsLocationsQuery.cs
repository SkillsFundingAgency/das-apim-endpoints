using MediatR;

namespace SFA.DAS.EmployerAan.Application.Onboarding.NotificationsLocations
{
    public class GetNotificationsLocationsQuery : IRequest<GetNotificationsLocationsQueryResult>
    {
        public long EmployerAccountId { get; set; }
        public string SearchTerm { get; set; }
    }
}

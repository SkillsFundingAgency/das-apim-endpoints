using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetReservations
{
    public class GetReservationsQueryResult
    {
        public IEnumerable<GetReservationsResponseListItem> Reservations { get; set; }
    }
}
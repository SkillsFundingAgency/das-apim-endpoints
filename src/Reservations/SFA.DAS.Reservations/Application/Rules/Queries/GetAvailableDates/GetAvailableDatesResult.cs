using System.Collections.Generic;
using SFA.DAS.Reservations.InnerApi.Responses;

namespace SFA.DAS.Reservations.Application.Rules.Queries.GetAvailableDates
{
    public class GetAvailableDatesResult
    {
        public IEnumerable<AvailableDateStartWindow> AvailableDates { get; set; }
    }
}

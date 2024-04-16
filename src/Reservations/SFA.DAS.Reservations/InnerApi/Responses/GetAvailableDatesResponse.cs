using System.Collections.Generic;

namespace SFA.DAS.Reservations.InnerApi.Responses
{
    public class GetAvailableDatesResponse
    {
        public IEnumerable<AvailableDateStartWindow> AvailableDates { get; set; }
    }
}

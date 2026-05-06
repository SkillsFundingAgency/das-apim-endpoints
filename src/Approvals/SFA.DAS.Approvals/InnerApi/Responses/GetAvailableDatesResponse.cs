using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetAvailableDatesResponse
    {
        public IEnumerable<AvailableDateStartWindow> AvailableDates { get; set; }
    }
}

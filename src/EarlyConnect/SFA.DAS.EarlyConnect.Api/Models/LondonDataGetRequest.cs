using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Api.Requests.GetRequests
{
    public class LondonDataGetRequest
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
    }
}

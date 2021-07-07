using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }
        public StandardDates StandardDates { get; set; }
    }
}

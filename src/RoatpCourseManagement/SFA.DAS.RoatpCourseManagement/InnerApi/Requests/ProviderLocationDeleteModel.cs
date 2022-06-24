using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class ProviderLocationDeleteModel
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public List<int> DeSelectedSubregionIds { get; set; } = new List<int>();
    }
}

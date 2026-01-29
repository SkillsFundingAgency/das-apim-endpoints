using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining
{
    public class GetStandardResponse
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }

    }
}
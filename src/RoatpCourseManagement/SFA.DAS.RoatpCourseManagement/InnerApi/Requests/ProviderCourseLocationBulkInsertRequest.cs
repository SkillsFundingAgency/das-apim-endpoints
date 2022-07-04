using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class ProviderCourseLocationBulkInsertRequest : IPostApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/locations/bulk-insert-regions";

        public ProviderCourseLocationBulkInsertRequest(ProviderCourseLocationBulkInsertModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            Data = data;
        }

        public object Data { get; set; }
    }
}

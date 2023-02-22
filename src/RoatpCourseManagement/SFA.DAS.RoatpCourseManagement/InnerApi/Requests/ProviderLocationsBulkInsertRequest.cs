using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class ProviderLocationsBulkInsertRequest : IPostApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string PostUrl => $"providers/{Ukprn}/locations/bulk-insert-regions";

        public ProviderLocationsBulkInsertRequest(ProviderLocationBulkInsertModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            UserDisplayName = data.UserDisplayName;
            Data = data;
        }

        public object Data { get; set; }
    }
}

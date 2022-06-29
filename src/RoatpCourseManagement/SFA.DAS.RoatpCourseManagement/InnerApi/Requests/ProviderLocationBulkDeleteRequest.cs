using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class ProviderLocationBulkDeleteRequest : IDeleteApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string DeleteUrl => $"/providers/{Ukprn}/locations/{LarsCode}/cleanup?userId={HttpUtility.UrlEncode(UserId)}";
        public ProviderLocationBulkDeleteRequest(ProviderLocationBulkDeleteModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
        }
    }
}

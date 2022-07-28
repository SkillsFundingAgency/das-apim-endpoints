using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class ProviderCourseUpdateRequest : IPutApiRequest<ProviderCourseUpdateModel>
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string PutUrl => $"providers/{Ukprn}/courses/{LarsCode}/";

        public ProviderCourseUpdateRequest(ProviderCourseUpdateModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            Data = data;
        }

        public ProviderCourseUpdateModel Data { get; set; }
    }
}

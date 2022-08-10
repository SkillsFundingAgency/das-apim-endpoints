using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProvider
{
    public class GetProviderResult
    {
        public string MarketingInfo { get; set; }

        public static implicit operator GetProviderResult(GetProviderResponse source) =>
            new GetProviderResult
            {
                MarketingInfo = source.MarketingInfo
            };
    }
}
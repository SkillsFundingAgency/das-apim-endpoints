
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;

namespace SFA.DAS.Roatp.CourseManagement.Application.Provider
{
    public class GetProviderResult
    {
        public InnerApi.Responses.Domain.Models.Provider Provider { get; set; }
        public GetProviderResult(GetProviderResponse provider)
        {
            Provider = provider;
        }
    }
}
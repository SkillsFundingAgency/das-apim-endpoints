
namespace SFA.DAS.Roatp.CourseManagement.Application.Provider
{
    public class GetProviderResult
    {
        public InnerApi.Responses.Domain.Models.Provider Provider { get; set; }
        public GetProviderResult(InnerApi.Responses.Domain.Models.Provider provider)
        {
            Provider = provider;
        }
    }
}
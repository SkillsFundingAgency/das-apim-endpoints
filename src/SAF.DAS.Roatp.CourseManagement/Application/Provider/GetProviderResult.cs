
namespace SFA.DAS.Roatp.CourseManagement.Application.Provider
{
    public class GetProviderResult
    {
        public InnerApi.Responses.Provider Provider { get; set; }
        public GetProviderResult(InnerApi.Responses.Provider provider)
        {
            Provider = provider;
        }
    }
}
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    class GetProviderCourseLocationsRequest : IGetApiRequest
    {
        public string GetUrl => $"providerCourseLocations/{ProviderCourseId}";
        public int ProviderCourseId { get; }

        public GetProviderCourseLocationsRequest(int providerCourseId)
        {
            ProviderCourseId = providerCourseId;
        }
    }
}


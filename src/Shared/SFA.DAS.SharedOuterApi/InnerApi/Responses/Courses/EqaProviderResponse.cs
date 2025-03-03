using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses
{
    [ExcludeFromCodeCoverage]
    public class EqaProviderResponse
    {
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string WebLink { get; set; }
    }
}

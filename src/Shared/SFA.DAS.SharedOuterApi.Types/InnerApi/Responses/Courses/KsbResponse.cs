using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses
{
    [ExcludeFromCodeCoverage]
    public class KsbResponse
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}

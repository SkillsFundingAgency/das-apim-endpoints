using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;

[ExcludeFromCodeCoverage]
public class RelatedOccupationResponse
{
    public string Title { get; set; }
    public int Level { get; set; }
}
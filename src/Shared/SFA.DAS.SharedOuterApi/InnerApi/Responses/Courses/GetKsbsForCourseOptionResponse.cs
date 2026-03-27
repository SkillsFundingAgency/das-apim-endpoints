using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;

public class GetKsbsForCourseOptionResponse
{
    public List<KsbResponse> Ksbs { get; set; } = [];
}
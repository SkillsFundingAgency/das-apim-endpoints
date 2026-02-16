using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.LearnerData.Responses;

public class GetProviderRelationshipQueryResponse
{
    public string Ukprn { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public EmployerDetails[] Employers { get; set; }
    public CourseTypes[] SupportedCourses { get; set; }
}
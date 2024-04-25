using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

public class GetAddQualificationQueryResult
{
    public QualificationReference? QualificationType { get; set; }
    public List<Qualification> Qualifications { get; set; }
    public List<CourseResponse> Courses { get; set; }

    public class CourseResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsStandard { get; set; }
    }
}
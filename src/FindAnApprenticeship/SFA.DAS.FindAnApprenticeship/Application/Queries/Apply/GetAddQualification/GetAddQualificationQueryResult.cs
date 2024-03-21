using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

public class GetAddQualificationQueryResult
{
    public QualificationReference? QualificationType { get; set; }
    public List<Qualification> Qualifications { get; set; }
}
using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

public class GetQualificationsQueryResult
{
    public bool? IsSectionCompleted { get; set; }
    public List<Qualification> Qualifications { get; set; }
    public List<QualificationReference> QualificationTypes { get; set; }
}
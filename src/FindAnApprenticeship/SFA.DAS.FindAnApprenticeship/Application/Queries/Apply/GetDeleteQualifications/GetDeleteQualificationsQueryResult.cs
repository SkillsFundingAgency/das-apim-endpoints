using System;
using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;

public class GetDeleteQualificationsQueryResult
{
    public Guid QualificationReference { get; set; }
    public List<Qualification> Qualifications { get; set; }

}
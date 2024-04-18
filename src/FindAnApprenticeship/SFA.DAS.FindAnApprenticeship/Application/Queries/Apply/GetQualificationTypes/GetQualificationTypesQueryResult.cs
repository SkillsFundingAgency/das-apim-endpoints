using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;

public class GetQualificationTypesQueryResult
{
    public bool HasAddedQualifications { get; set; }
    public List<QualificationReference> QualificationTypes { get; set; }
}
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public class GetQualificationReferenceTypesApiResponse
{
    [JsonPropertyName("qualificationReferences")]
    public List<QualificationReference> QualificationReferences { get; set; }
}
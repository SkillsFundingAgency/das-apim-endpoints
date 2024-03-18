using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public class GetQualificationReferenceTypesApiResponse
{
    [JsonPropertyName("qualificationReferences")]
    public List<QualificationReference> QualificationReferences { get; set; }
}

public class QualificationReference
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("order")]
    public long Order { get; set; }
}
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public class GetQualificationReferenceTypesApiResponse
{
    [JsonProperty("qualificationReferences")]
    public List<QualificationReference> QualificationReferencesQualificationReferences { get; set; }
}

public class QualificationReference
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("order")]
    public long Order { get; set; }
}
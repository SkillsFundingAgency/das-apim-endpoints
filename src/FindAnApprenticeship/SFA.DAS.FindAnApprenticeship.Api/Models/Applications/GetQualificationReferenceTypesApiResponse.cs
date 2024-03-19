using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetQualificationReferenceTypesApiResponse
{
    public bool HasAddedQualifications { get; set; }

    [JsonPropertyName("QualificationTypes")]
    public List<GetQualificationTypeApiResponse> QualificationTypes { get; set; }
    public static implicit operator GetQualificationReferenceTypesApiResponse(GetQualificationTypesQueryResult source)
    {
        return new GetQualificationReferenceTypesApiResponse()
        {
            HasAddedQualifications = source.HasAddedQualifications,
            QualificationTypes = source.QualificationTypes.Select(c=>(GetQualificationTypeApiResponse)c).ToList()
        };
    }
}

public class GetQualificationTypeApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("order")]
    public long Order { get; set; }

    public static implicit operator GetQualificationTypeApiResponse(QualificationReference source)
    {
        return new GetQualificationTypeApiResponse
        {
            Id = source.Id,
            Name = source.Name,
            Order = source.Order
        };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetQualificationReferenceTypesApiResponse
{
    [JsonProperty("QualificationTypes")]
    public List<GetQualificationTypeApiResponse> QualificationTypes { get; set; }
    public static implicit operator GetQualificationReferenceTypesApiResponse(GetQualificationTypesQueryResult source)
    {
        return new GetQualificationReferenceTypesApiResponse()
        {
            QualificationTypes = source.QualificationTypes.Select(c=>(GetQualificationTypeApiResponse)c).ToList()
        };
    }
}

public class GetQualificationTypeApiResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("order")]
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
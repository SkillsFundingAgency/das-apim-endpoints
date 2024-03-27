using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;

public class GetQualificationReferenceTypesApiResponse
{
    public bool HasAddedQualifications { get; set; }

    [JsonPropertyName("QualificationTypes")]
    public List<QualificationTypeApiResponse> QualificationTypes { get; set; }
    public static implicit operator GetQualificationReferenceTypesApiResponse(GetQualificationTypesQueryResult source)
    {
        return new GetQualificationReferenceTypesApiResponse()
        {
            HasAddedQualifications = source.HasAddedQualifications,
            QualificationTypes = source.QualificationTypes.Select(c=>(QualificationTypeApiResponse)c).ToList()
        };
    }
}
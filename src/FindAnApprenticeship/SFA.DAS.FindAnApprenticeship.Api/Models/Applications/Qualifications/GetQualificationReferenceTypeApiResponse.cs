using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;

public class GetQualificationReferenceTypeApiResponse
{
    [JsonPropertyName("QualificationType")]
    public QualificationTypeApiResponse QualificationType { get; set; }
    
    public List<ApplicationQualificationApiResponse> Qualifications { get; set; }

    public static implicit operator GetQualificationReferenceTypeApiResponse(GetAddQualificationQueryResult source)
    {
        return new GetQualificationReferenceTypeApiResponse
        {
            QualificationType = source.QualificationType,
            Qualifications = source.Qualifications.Select(c=>(ApplicationQualificationApiResponse)c).ToList()
        };
    }
}
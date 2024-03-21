using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetQualificationReferenceTypeApiResponse
{
    [JsonPropertyName("QualificationType")]
    public GetQualificationTypeApiResponse QualificationType { get; set; }

    public static implicit operator GetQualificationReferenceTypeApiResponse(GetAddQualificationQueryResult source)
    {
        return new GetQualificationReferenceTypeApiResponse
        {
            QualificationType = source.QualificationType,
            //Qualifications = source.Qualifications
        };
    }
}
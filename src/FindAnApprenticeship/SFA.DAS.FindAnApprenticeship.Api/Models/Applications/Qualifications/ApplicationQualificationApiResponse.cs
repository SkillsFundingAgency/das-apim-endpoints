using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;

public class ApplicationQualificationApiResponse
{
    public Guid Id { get; set; }
    public string? Subject { get; set; }
    public string? Grade { get; set; }
    public string? AdditionalInformation { get; set; }
    public bool? IsPredicted { get; set; }
    public Guid QualificationReferenceId { get; set; }
    public QualificationTypeApiResponse QualificationReference { get; set; }

    public static implicit operator ApplicationQualificationApiResponse(Qualification source)
    {
        return new ApplicationQualificationApiResponse
        {
            Id = source.Id,
            AdditionalInformation = source.AdditionalInformation,
            Grade = source.Grade,
            IsPredicted = source.IsPredicted,
            Subject = source.Subject,
            QualificationReferenceId = source.QualificationReference.Id,
            QualificationReference = source.QualificationReference
        };
    }
}
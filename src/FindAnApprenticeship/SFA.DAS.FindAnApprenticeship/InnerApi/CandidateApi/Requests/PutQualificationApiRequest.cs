using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class PutQualificationApiRequest(Guid candidateId, Guid applicationId, PutQualificationApiRequestData data) : IPutApiRequest
{
    public string PutUrl => $"api/candidates/{candidateId}/applications/{applicationId}/Qualifications";
    public object Data { get; set; } = data;
}

public class PutQualificationApiRequestData
{
    public Guid Id { get; set; }
    public int? ToYear { get; set; }
    public string? Grade { get; set; }
    public string? Subject { get; set; }
    public bool? IsPredicted { get; set; }
    public string? AdditionalInformation { get; set; }
    public Guid QualificationReferenceId { get; set; }
}
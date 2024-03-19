using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class UpdateApplicationQualificationRequest
{
    public Guid Id { get; set; }
    public int? ToYear { get; set; }
    public string? Grade { get; set; }
    public string? Subject { get; set; }
    public bool? IsPredicted { get; set; }
    public string? AdditionalInformation { get; set; }
}
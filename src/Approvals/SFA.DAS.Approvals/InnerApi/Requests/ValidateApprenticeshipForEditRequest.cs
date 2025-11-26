using SFA.DAS.Approvals.Application.Shared.Enums;
using System;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class ValidateApprenticeshipForEditRequest
{
    public long ApprenticeshipId { get; set; }
    public long? EmployerAccountId { get; set; }
    public long? ProviderId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal? Cost { get; set; }
    public string EmployerReference { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ULN { get; set; }
    public int? DeliveryModel { get; set; }
    public string TrainingCode { get; set; }
    public string ProviderReference { get; set; }
    public string Email { get; set; }
    public string Version { get; set; }
    public string Option { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public int? EmploymentPrice { get; set; }
    public int MinimumAgeAtApprenticeshipStart { get; set; }
    public int MaximumAgeAtApprenticeshipStart { get; set; }
    public Party Party {  get; set; }
}
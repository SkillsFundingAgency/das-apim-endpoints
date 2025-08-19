using System;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class ConfirmEditApprenticeshipRequest
{
    public long ApprenticeshipId { get; set; }
    public long? ProviderId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? Cost { get; set; }
    public string ProviderReference { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DeliveryModel { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public int? EmploymentPrice { get; set; }
    public string CourseCode { get; set; }
    public string Version { get; set; }
    public string Option { get; set; }
    public UserInfo UserInfo { get; set; }
} 
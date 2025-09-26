using System;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class EditApprenticeshipApiRequest(EditApprenticeshipApiRequestData editApprenticeshipApiRequestData)
    : IPostApiRequest
{
    public EditApprenticeshipApiRequestData EditApprenticeshipApiRequestData { get; set; } = editApprenticeshipApiRequestData;
    public string PostUrl => "api/apprenticeships/edit";
    public object Data { get; set; } = editApprenticeshipApiRequestData;
}

public class EditApprenticeshipApiRequestData
{ 
    public long ApprenticeshipId { get; set; }
    public long? ProviderId { get; set; }
    public long? AccountId { get; set; }
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
    public int? MinimumAgeAtApprenticeshipStart { get; set; }
    public int? MaximumAgeAtApprenticeshipStart { get; set; }
    public UserInfo UserInfo { get; set; }
    public Party Party { get; set; }    
    public string EmployerReference { get; set; }
} 
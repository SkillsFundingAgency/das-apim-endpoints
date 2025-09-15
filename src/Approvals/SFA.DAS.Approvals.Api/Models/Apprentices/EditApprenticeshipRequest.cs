using System;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class EditApprenticeshipRequest
{
    public long ApprenticeshipId { get; set; }
    public long? EmployerAccountId { get; set; }
    public long ProviderId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string ULN { get; set; }
    public string CourseCode { get; set; }
    public string Version { get; set; }
    public string Option { get; set; }
    public decimal? Cost { get; set; }
    public decimal? TrainingPrice { get; set; }
    public decimal? EndPointAssessmentPrice { get; set; }
    public string EmployerReference { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int DeliveryModel { get; set; }
    public string ProviderReference { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public int? EmploymentPrice { get; set; }
    public string ChangeCourse { get; set; }
    public string ChangeDeliveryModel { get; set; }
}
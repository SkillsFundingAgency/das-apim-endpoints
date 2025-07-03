using System;
using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.EditApprenticeship;

public class EditApprenticeshipCommand : IRequest<EditApprenticeshipResult>
{
    public long ApprenticeshipId { get; set; }
    public long? EmployerAccountId { get; set; }
    public long ProviderId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal? Cost { get; set; }
    public string EmployerReference { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; }
    public string ULN { get; set; }
    public string CourseCode { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? EmploymentEndDate { get; set; }
    public int? DeliveryModel { get; set; }
    public string ProviderReference { get; set; }
    public string Version { get; set; }
    public string Option { get; set; }
    public int? EmploymentPrice { get; set; }
}
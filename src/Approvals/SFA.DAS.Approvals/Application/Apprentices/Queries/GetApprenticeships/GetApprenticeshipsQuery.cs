using System;
using MediatR;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.InnerApi;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;

public class GetApprenticeshipsQuery : IRequest<GetApprenticeshipsQueryResult>
{
    public long? AccountId { get; set; }
    public long? ProviderId { get; set; }
    public int PageNumber { get; set; }
    public int PageItemCount { get; set; }
    public string SortField { get; set; }
    public bool ReverseSort { get; set; }
    public string SearchTerm { get; set; }
    public string EmployerName { get; set; }
    public string ProviderName { get; set; }
    public string CourseName { get; set; }
    public ApprenticeshipStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? AccountLegalEntityId { get; set; }
    public DateTime? StartDateRangeFrom { get; set; }
    public DateTime? StartDateRangeTo { get; set; }
    public Alerts? Alert { get; set; }
    public ConfirmationStatus? ApprenticeConfirmationStatus { get; set; }
    public DeliveryModel? DeliveryModel { get; set; }
}
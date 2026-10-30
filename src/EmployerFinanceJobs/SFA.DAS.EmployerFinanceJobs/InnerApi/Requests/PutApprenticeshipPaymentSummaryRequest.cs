using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record PutApprenticeshipPaymentSummaryRequest(long AccountId, long ApprenticeshipId, PutApprenticeshipPaymentSummaryRequestData Payload) : IPutApiRequest
{
    public string PutUrl => $"api/employer/{AccountId}/apprenticeship-payment-summary/{ApprenticeshipId}";
    public object Data { get; set; } = Payload;
}

public sealed record PutApprenticeshipPaymentSummaryRequestData
{
    public bool IsTransfer { get; set; }
    public long Uln { get; set; }
    public DateOnly Dob { get; set; }
    public ApprenticeshipStatus Status { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalPaid { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public long ProviderId { get; set; }
    public int ProviderOrder { get; set; } = 0;
    public DateTime ApprovalDate { get; set; }
    public LearningType LearningType { get; set; }
    public DateOnly? LastPaymentDate { get; set; }
    public decimal? LastPaymentAmount { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
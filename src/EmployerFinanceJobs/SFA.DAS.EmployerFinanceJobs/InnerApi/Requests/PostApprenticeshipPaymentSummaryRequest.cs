using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;

public sealed record PostApprenticeshipPaymentSummaryRequest(long AccountId, long ApprenticeshipId, PostApprenticeshipPaymentSummaryRequestData Payload) : IPostApiRequest
{
    public string PostUrl => $"api/employers/{AccountId}/apprenticeship-payment-summary/{ApprenticeshipId}";
    public object Data { get; set; } = Payload;
}

public sealed record PostApprenticeshipPaymentSummaryRequestData
{
    public bool IsTransfer { get; set; }
    public long Uln { get; set; }
    public DateOnly? Dob { get; set; }
    public ApprenticeshipStatus Status { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalPaid { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly? LastPaymentDate { get; set; }
    public decimal? LastPaymentAmount { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
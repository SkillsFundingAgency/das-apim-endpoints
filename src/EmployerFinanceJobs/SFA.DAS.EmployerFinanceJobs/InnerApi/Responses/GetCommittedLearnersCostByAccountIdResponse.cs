using SFA.DAS.EmployerFinanceJobs.Domain.Enums;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record GetCommittedLearnersCostByAccountIdResponse
{
    public IEnumerable<ApprenticeshipDetailsResponse> Apprenticeships { get; set; } = [];
    public int TotalApprenticeshipsFound { get; set; }
    public int TotalApprenticeshipsWithAlertsFound { get; set; }
    public int TotalApprenticeships { get; set; }
    public int PageNumber { get; set; }
    public bool HasChangeHistory { get; set; }

    public sealed record ApprenticeshipDetailsResponse
    {
        public long Id { get; set; }
        public string Uln { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ProviderId { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public LearningType LearningType { get; set; }
        public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
        public decimal? TotalAgreedPrice { get; set; }
        public long? TransferSenderId { get; set; }
        public decimal? Cost { get; set; }
        public bool HasChangeHistory { get; set; }
        public DateTime? FreezedDate { get; set; }
    }
}

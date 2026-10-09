using SFA.DAS.EmployerFinance.Models.Enums;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses.Commitments;

public sealed record GetCommittedLearnersCostByAccountIdResponse
{
    public IEnumerable<ApprenticeshipDetailsResponse> Apprenticeships { get; set; }
    public int TotalApprenticeshipsFound { get; set; }
    public int TotalApprenticeshipsWithAlertsFound { get; set; }
    public int TotalApprenticeships { get; set; }
    public int PageNumber { get; set; }
    public bool HasChangeHistory { get; set; }

    public class ApprenticeshipDetailsResponse
    {
        public long Id { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
        public decimal? TotalAgreedPrice { get; set; }
        public long? TransferSenderId { get; set; }
        public decimal? Cost { get; set; }
        public bool HasChangeHistory { get; set; }
    }
}
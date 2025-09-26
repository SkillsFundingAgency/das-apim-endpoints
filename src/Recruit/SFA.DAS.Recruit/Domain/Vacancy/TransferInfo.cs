using System;

namespace SFA.DAS.Recruit.Domain.Vacancy;

public class TransferInfo
{
    public long Ukprn { get; set; }
    public string ProviderName { get; set; }
    public string LegalEntityName { get; set; }
    public VacancyUser TransferredByUser { get; set; }
    public DateTime TransferredDate { get; set; }
    public TransferReason Reason { get; set; }
}